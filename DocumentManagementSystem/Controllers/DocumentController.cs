using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using DocumentManagementSystem.Models;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        [HttpGet]
        public ActionResult AddDocument()
        {
            DocumentModel documentModel = new DocumentModel(); 
            documentModel.selectDocumentTypeItems= getDocumentTypeValue();
            return View(documentModel);
        }

        private string GenerateVerificationCode()
        {
            string uniqueCode = Guid.NewGuid().ToString().ToUpper().Replace("-", "").Replace("0", "Z").Replace("O", "M");
            string part1 = uniqueCode.Substring(0, 4);
            string part2 = uniqueCode.Substring(10, 4);
            string part3 = uniqueCode.Substring(20, 4);
            return  part1 + part2 + part3;
        }

        [HttpPost]
        public ActionResult AddDocument(DocumentModel documentModel )
        {
            //TODO : Authorization sisteminde geçince id sistem tarafından atansın!
            documentModel.document.AdminID = 1;
            documentModel.document.DocumentCreateDate= DateTime.Parse(DateTime.Now.ToShortDateString());
            documentModel.document.DocumentStatus = false;
            documentModel.document.DocumentPdfUrl = "";
            documentModel.document.DocumentVerificationCode = GenerateVerificationCode();
            /*
            Student student = StudentManager.GetStudentWihtNumber(documentModel.document.StudentID.ToString());
            documentModel.document.StudentID = student.StudentID;
            */
            documentModel.document.StudentID = 1;
            int documetnID = documentManager.DocumentAdd(documentModel.document);
            
             

            List<DocumentTypeSignature> signatureList = DocumentTypeSignatureManager.GetListToDocumentTypeSignature(documentModel.document.DocumentTypeID);
            foreach (DocumentTypeSignature signature in signatureList)
            {
                DocumentSignature documentSignature = new DocumentSignature();
                documentSignature.DocumentSignatureStatus = false;
                documentSignature.DocumentTypeSignatureID = signature.DocumentTypeSignatureID;
                documentSignature.DocumentID = documetnID;
                DocumentSignatureManager.DocumentSignatureAdd(documentSignature);
            }





            return View();
        }

        #endregion
    }
}