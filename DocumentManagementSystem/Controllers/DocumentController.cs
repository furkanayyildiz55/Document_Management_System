using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Concrete.Repositories;
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
    //[Authorize(Roles="0,1")]
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
            int AdminID = (int)Session["UserID"];
            Repository repo = new Repository();
            DocumentSignModel documentSignModel = new DocumentSignModel();
            documentSignModel.SignedDocumentList = repo.DocumentSignaturesWithAdminID(AdminID);
            return View( documentSignModel);
        }

        [HttpPost]
        public ActionResult DocumentSign(DocumentSignModel documentSignModel)
        {
            int AdminID = (int)Session["UserID"];

            //Belge admin tarafından imzalanıyor
            DocumentSignature documentSignature = DocumentSignatureManager.GetDocumentSignature(int.Parse(documentSignModel.DocumentSignatureID));
            documentSignature.DocumentSignatureStatus = true;
            DocumentSignatureManager.DocumentSignatureUpdate(documentSignature);

            //imzalanmamış belge varmı kontrolü yapılıyor
            //imzalanmamış belge kalmadı ise belge durumunu True yapacak

            bool AllDocumentSignatureStatus = DocumentSignatureManager.GetAllDocumentSignatureStatus(int.Parse(documentSignModel.DocumentID));

            //True ise belgedeki tüm imzalar onaylanmış 
            if (AllDocumentSignatureStatus)
            {
                Document document = documentManager.GetDocument(int.Parse(documentSignModel.DocumentID));
                document.DocumentStatus = true;
                documentManager.DocumentUpdate(document);
                GeneratePDFFunc(int.Parse(documentSignModel.DocumentID));
            }
            return RedirectToAction("DocumentSign", "Document");
        }

        #endregion

        #region DocumentList

        [HttpGet]
        public ActionResult DocumentList()
        {
            DocumentListModel documentListModel = new DocumentListModel();
            documentListModel.DocumentList= documentManager.GetList();
            documentListModel.DocumentList.Reverse();
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

            documentModel.document.AdminID = (int)Session["UserID"];
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

                    if (student == null) student = StudentManager.GetStudentWithMail(documentModel.studentNo);

                    if (student == null)
                    {
                        ViewBag.RecordStatus = false;
                        ModelState.AddModelError("StudentNo", "Öğrenci bulunamadı, geçerli bir numara veya mail giriniz !");
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
                        ModelState.AddModelError("StudentNo", "Lütfen Öğrenci Numarası veya Maili Giriniz !");
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


        [HttpGet]
        public ActionResult VievPDFDocument(int? id)
        {
            int? DocumentID = id;
            try
            {
                if(DocumentID != null && DocumentID > 0) 
                {
                    Document document = documentManager.GetDocument(DocumentID);
                    if (document.DocumentStatus)
                    {
                        if(document.DocumentPdfUrl != "")
                        {
                            ViewBag.PDFUrl = document.DocumentPdfUrl;
                            return View();
                        }
                        else
                        {
                            ViewBag.PDFUrl = null;
                            ViewBag.Error = "Hatalı belge. Oluşturulan belge imzalanmış fakat PDF oluşturulmamış.";
                            return View();
                        }

                    }
                    else 
                    {
                        ViewBag.PDFUrl = null;
                        ViewBag.Error = "Bu belge imzalanmamış ve PDF dosyası oluşturulmamış !";
                        return View();                    
                    }

                }
                else
                {
                    ViewBag.PDFUrl = null;
                    ViewBag.Error = "Bir hata oluştu, bu link geçerli değil !";
                    return View();
                }

            }
            catch (Exception )
            {
                ViewBag.Error = "Bir hata oluştu. Lütfen yetkililere bildiriniz !";
                return View();
            }

        }

        [HttpGet]
        public ActionResult GeneratePDF(int? id)
        {

            bool isGeneratePDF = GeneratePDFFunc(id);
            
            if(isGeneratePDF)
                return RedirectToAction("AddDocument", "Document");
            else
            {
                return View();
            }
        }

        private bool GeneratePDFFunc(int? DocumentID)
        {
            if (DocumentID != null && DocumentID < 0) return false;

            DocumentCreateModel documentCreateModel = DocumentCreateModelGetData(DocumentID);

            if (documentCreateModel != null)
            {
                //foreach (var item in documentCreateModel.documentCreateSignatures)
                //{
                //    if (item.SignatureStatus == false) //onaylanmamış imza kontrolü
                //    {
                //       // return RedirectToAction("AddDocument", "Document");
                //    }
                //}
            }
            else
            {
                return false;
            }

            try
            {
                var report = new ViewAsPdf("VievHTMLDocument", documentCreateModel)
                {
                    PageOrientation = Rotativa.Options.Orientation.Landscape,
                    PageSize = Rotativa.Options.Size.A4,
                    PageMargins = new Margins(0, 0, 0, 0),
                };

                string fileName = $"{documentCreateModel.StudentFullName} {documentCreateModel.DocumentName} {documentCreateModel.DocumentVerificationCode}.pdf";
                string fullPath = "/DocumentsPDF/" + fileName;
                string serverPath = Server.MapPath("~" + fullPath);

                Document document = documentManager.GetDocument(int.Parse(documentCreateModel.DocumentID));
                document.DocumentPdfUrl = fullPath;
                documentManager.DocumentUpdate(document);

                var byteArray = report.BuildPdf(this.ControllerContext);
                var fileStream = new FileStream(serverPath, FileMode.Create, FileAccess.Write);
                fileStream.Write(byteArray, 0, byteArray.Length);
                fileStream.Close();

                return true;
            }
            catch (Exception)
            {
                //TODO : hata sayfasına hata kodu ile yönlendir
                throw;
            }
            
        }

        private DocumentCreateModel DocumentCreateModelGetData(int? DocumentID)
        {
            Document document = documentManager.GetDocument(DocumentID);

            if(document == null)
            {
                return null;
            }

            Student student = StudentManager.GetStudent(document.StudentID);
            DocumentType documentType = documentTypeManager.GetDocumentType(document.DocumentTypeID);
            DocumentTypeSignature documentTypeSignature = DocumentTypeSignatureManager.GetDocumentTypeSignature(documentType.DocumentTypeID);
            List<DocumentSignature> documentSignatureList = DocumentSignatureManager.GetList(document.DocumentID);

            DocumentCreateModel documentCreateModel = new DocumentCreateModel();
            documentCreateModel.StudentFullName = $"{student.StudentName} {student.StudentSurname}";
            documentCreateModel.StudentNoMail = student.StudentNo != null ? student.StudentNo.ToString() : student.StudentMail;
            documentCreateModel.StudentProgram = student.StudentProgram;

            documentCreateModel.DocumentID = document.DocumentID.ToString();
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