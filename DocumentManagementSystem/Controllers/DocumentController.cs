using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using DocumentManagementSystem.Models;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Rotativa;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace DocumentManagementSystem.Controllers
{
    public class DocumentController : Controller
    {

        DocumentManager documentManager = new DocumentManager(new EfDocumentDal());
        DocumentTypeManager documentTypeManager = new DocumentTypeManager(new EfDocumentTypeDal());
        DocumentTypeSignatureManager DocumentTypeSignatureManager = new DocumentTypeSignatureManager(new EfDocumentTypeSignatureDal());
        DocumentSignatureManager DocumentSignatureManager = new DocumentSignatureManager(new EfDocumentSignatureDal());
        StudentManager StudentManager = new StudentManager(new EfStudentDal());
        AdminManager AdminManager = new AdminManager(new EfAdminDal());

        #region DocumentSign

        [HttpGet]
        public ActionResult DocumentSign()
        {
            //DataAccessLayer.Concrete.Repositories.Repository repository = new DataAccessLayer.Concrete.Repositories.Repository();
            //repository.DocumentSignaturesWithAdminID(4);

            int AdminID = 1;

            List<DocumentTypeSignature> documentTypeSignatureList = DocumentTypeSignatureManager.GetListToDocumentTypeSignatureWithAdminID(AdminID);

            List<DocumentSignature> documentSignaatureList = new List<DocumentSignature>();

            foreach (var item in documentTypeSignatureList)
            {
                List<DocumentSignature> documentSignatures = DocumentSignatureManager.GetListWithDocumentTypeSignatureID(item.DocumentTypeSignatureID);
                foreach (var item2 in documentSignatures)
                {
                    if(item2.Document.DocumentStatus == false)
                    {
                        documentSignaatureList.Add(item2);
                    }
                }
            }

            return View(documentSignaatureList);
        }

        [HttpPost]
        public ActionResult DocumentSign(int a)
        {
            return View();
        }

        #endregion

        #region DocumentTypeList

        [HttpGet]
        public ActionResult DocumentList()
        {
            DocumentListModel documentListModel = new DocumentListModel();
            documentListModel.DocumentList= documentManager.GetList();
            return View(documentListModel);

        }


        [HttpPost]
        public ActionResult DocumentList(DocumentListModel documentListModel)
        {
            documentListModel.DocumentList = documentManager.GetList(documentListModel.StudentNo);
            return View(documentListModel);
        }

        #endregion

        #region AddDocument

        #region Methods
        private List<SelectListItem> getDocumentTypeValue()
        {
            return (from x in documentTypeManager.GetListActiveDocument()
                    select new SelectListItem
                    {
                        Text = x.DocumentTypeName,
                        Value = x.DocumentTypeID.ToString()
                    }).ToList();
        }
        private string GenerateVerificationCode()
        {
            string uniqueCode = Guid.NewGuid().ToString().ToUpper().Replace("-", "").Replace("0", "Z").Replace("O", "M");
            string part1 = uniqueCode.Substring(0, 4);
            string part2 = uniqueCode.Substring(10, 4);
            string part3 = uniqueCode.Substring(20, 4);
            return part1 + part2 + part3;
        }
        #endregion

        [HttpGet]
        public ActionResult AddDocument()
        {
            DocumentModel documentModel = new DocumentModel();
            documentModel.selectDocumentTypeItems = getDocumentTypeValue();
            return View(documentModel);
        }


        [HttpPost]
        public ActionResult AddDocument(DocumentModel documentModel)
        {
            DocumentValidator documentValidator = new DocumentValidator();
            ValidationResult result = documentValidator.Validate(documentModel.document);
            documentModel.selectDocumentTypeItems = getDocumentTypeValue();

            //TODO : Authorization sisteminde geçince id sistem tarafından atansın!
            documentModel.document.AdminID = 1;
            documentModel.document.DocumentCreateDate = DateTime.Parse(DateTime.Now.ToString());
            documentModel.document.DocumentStatus = false;
            documentModel.document.DocumentPdfUrl = "";
            documentModel.document.DocumentVerificationCode = GenerateVerificationCode();


            bool StudentNoisNotNull = (documentModel.studentNo == null ? false: true);  //null değil ise true olacak
            try
            {
                
                if( result.IsValid && StudentNoisNotNull)
                {
                    //öğrenci verileri çekiliyor
                    var student = StudentManager.GetStudentWihtNumber(documentModel.studentNo);
                    if (student == null)
                    {
                        ViewBag.RecordStatus = false;
                        ModelState.AddModelError("StudentNo", "Öğrenci bulunamadı, geçerli bir numara giriniz !");
                        ViewBag.RecordStatus = false;
                        return View(documentModel);
                    }
                    else
                    {
                        documentModel.document.StudentID = student.StudentID;
                       
                        //belge ekleniyor ve idsi çekiliyor
                        int documetnID = documentManager.DocumentAdd(documentModel.document);

                        //belge türü verileri çekiliyor
                        List<DocumentTypeSignature> signatureList = DocumentTypeSignatureManager.GetListToDocumentTypeSignature(documentModel.document.DocumentTypeID);

                        //belgeye imza ekleme işlemleri yapılıyor
                        foreach (DocumentTypeSignature signature in signatureList)
                        {
                            DocumentSignature documentSignature = new DocumentSignature();
                            documentSignature.DocumentSignatureStatus = false;
                            documentSignature.DocumentTypeSignatureID = signature.DocumentTypeSignatureID;
                            documentSignature.DocumentID = documetnID;
                            DocumentSignatureManager.DocumentSignatureAdd(documentSignature);
                        }
                        ViewBag.RecordStatus = true;
                    }
                }
                else
                {
                    ViewBag.RecordStatus = false;
                    if (!StudentNoisNotNull)
                    {
                        ModelState.AddModelError("StudentNo", "Lütfen Öğrenci Numarası Giriniz !");
                    }
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
                return View(documentModel); ;
            }
            catch (Exception e)
            {
                ViewBag.RecordStatus = false;
                ModelState.AddModelError("PageError", $"Bilinmeyen hata gerçekleşti: {e.Message} ");
            }
            documentModel.selectDocumentTypeItems = getDocumentTypeValue();
            return View(documentModel);
        }

        #endregion

        #region CreateandVievDocument

        [HttpGet]
        public ActionResult VievHTMLDocument (int? id)
        {
            //TODO :  ID gözükmeyecek şekilde sayfa çalıştırılacak  

            if (id != null && id >= 0)
            {
                DocumentCreateModel documentCreateModel = DocumentCreateModelGetData((int)id);
                return View(documentCreateModel);
            }
            else
            {
                return View();
            }
        }
   
        public ActionResult GeneratePDF()
        {
            int DocumentID = 24;
            DocumentCreateModel documentCreateModel = DocumentCreateModelGetData(DocumentID);

            if (documentCreateModel != null)
            {
                foreach (var item in documentCreateModel.documentCreateSignatures)
                {
                    if (item.SignatureStatus == false)
                    {
                       // return RedirectToAction("AddDocument", "Document");
                    }
                }
            }
            else
            {
                return RedirectToAction("AddDocument", "Document");

            }


            var report =  new ViewAsPdf("VievHTMLDocument", documentCreateModel )
            {
                //FileName = Server.MapPath("~/Content/DENEME.pdf" ),
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Margins(0, 0, 0, 0),
            };

            string fileName =  $"{documentCreateModel.StudentFullName} {documentCreateModel.DocumentName} {documentCreateModel.DocumentVerificationCode}.pdf";
            string fullPath =  Server.MapPath("~/DocumentsPDF/" + fileName);


            var byteArray = report.BuildPdf(ControllerContext);
            var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
            fileStream.Write(byteArray, 0, byteArray.Length);
            fileStream.Close();

            //if (!System.IO.File.Exists(fullPath))
            //{
            //}

            ViewBag.PDF = "/DocumentsPDF/" + fileName ;
            return View();
        }

        private DocumentCreateModel DocumentCreateModelGetData(int DocumentID)
        {
            
            Document document = documentManager.GetDocument(DocumentID);

            if(document == null)
            {
                return null;
            }

           // var item = document.Student;

            Student student = StudentManager.GetStudent(document.StudentID);
            DocumentType documentType = documentTypeManager.GetDocumentType(document.DocumentTypeID);
            DocumentTypeSignature documentTypeSignature = DocumentTypeSignatureManager.GetDocumentTypeSignature(documentType.DocumentTypeID);
            List<DocumentSignature> documentSignatureList = DocumentSignatureManager.GetList(document.DocumentID);

            DocumentCreateModel documentCreateModel = new DocumentCreateModel();
            documentCreateModel.StudentFullName = $"{student.StudentName} {student.StudentSurname}";
            documentCreateModel.StudentNoMail = student.StudentNo != null ? student.StudentNo.ToString() : student.StudentMail;
            documentCreateModel.StudentProgram = student.StudentProgram;

            documentCreateModel.DocumentName = documentType.DocumentTypeName;
            documentCreateModel.DocumentText = document.DocumentAlternativeText != null ? document.DocumentAlternativeText : documentType.DocumentTypeText;
            documentCreateModel.DocumentVerificationCode = document.DocumentVerificationCode;
            documentCreateModel.DocumentCreateDate = document.DocumentCreateDate.ToShortDateString();
            documentCreateModel.DocumentBacgrounImage = documentType.DocumentTypeBacgroundImage;

            foreach (var documentSignature in documentSignatureList)
            {
                DocumentCreateSignature documentCreateSignature = new DocumentCreateSignature();

                DocumentTypeSignature documentTypeSignature1 = DocumentTypeSignatureManager.GetDocumentTypeSignature(documentTypeSignature.DocumentTypeSignatureID);
                Admin admin = AdminManager.GetAdmin((int)documentTypeSignature1.AdminID);

                documentCreateSignature.AdminFullName = $"{admin.AdminName} {admin.AdminSurmane}";
                documentCreateSignature.AdminJob = admin.AdminJob;
                documentCreateSignature.AdminSignatureImage = admin.AdminSignatureImage;
                documentCreateSignature.SignatureStatus = documentSignature.DocumentSignatureStatus;
                documentCreateSignature.SignatureAlign = documentCreateSignature.SignatureAlign != null ? documentCreateSignature.SignatureAlign : "";

                documentCreateModel.documentCreateSignatures.Add(documentCreateSignature);
            }
            return documentCreateModel;
        }


        #endregion

        #region DocumentVerification

        [HttpGet]
        public ActionResult DocumentVerification()
        {
            DocumentVerificationModel documentVerificationModel = new DocumentVerificationModel();
            documentVerificationModel.isPostMethod = false;
            return View(documentVerificationModel);
        }

        [HttpPost]
        public ActionResult DocumentVerification(DocumentVerificationModel documentVerificationModel)
        {
            DocumentVerificationValidator documentVerificationValidator = new DocumentVerificationValidator();
            Document ValidationDocument = new Document();
            ValidationDocument.DocumentVerificationCode = documentVerificationModel.VerificationCode;
            ValidationResult result= documentVerificationValidator.Validate(ValidationDocument);

            if (result.IsValid)
            {
               documentVerificationModel.document  = documentManager.GetDocumentWithVerificationCode(documentVerificationModel.VerificationCode);
               documentVerificationModel.isPostMethod = true;

            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }

            return View(documentVerificationModel);
        }

        #endregion


    }
}