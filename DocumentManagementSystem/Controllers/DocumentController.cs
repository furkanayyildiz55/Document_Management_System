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

        #region AddDocument
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


        #region CreateDocument

        [HttpGet]
        public ActionResult CreateDocument ()
        {
            int DocumentID = 23;
            Document document = documentManager.GetDocument(DocumentID);

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
                documentCreateSignature.SignatureAlign = documentCreateSignature.SignatureAlign !=null ? documentCreateSignature.SignatureAlign : "";
                
                documentCreateModel.documentCreateSignatures.Add(documentCreateSignature);
            }
            ViewBag.data = documentCreateModel;

            

            return View(documentCreateModel);
        }

        [HttpPost]
        public ActionResult CreateDocument(Document d)
        {
            //PDF KAYDEDİLECEK
            return View();
        }



        public ActionResult GeneratePDF(string VerificationCode)
        {
            var report =  new ActionAsPdf("CreateDocument")
            {
                FileName = Server.MapPath("~/Content/Relato.pdf" ),
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Margins(0, 0, 0, 0),
            };

            string fileName =  VerificationCode+"Test.pdf";
            string fullPath =  Server.MapPath("~/image/" + fileName);

            if (!System.IO.File.Exists(fullPath))
            {
                var byteArray = report.BuildPdf(ControllerContext);
                var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                fileStream.Write(byteArray, 0, byteArray.Length);
                fileStream.Close();
            }
            return report;
        }

        #endregion
    }
}