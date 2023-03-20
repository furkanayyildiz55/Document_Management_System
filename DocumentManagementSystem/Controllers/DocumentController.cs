using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using DocumentManagementSystem.Models;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
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
    }
}