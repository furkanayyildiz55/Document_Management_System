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
            documentModel.document.DocumentCreateDate= DateTime.Parse(DateTime.Now.ToShortDateString());
            documentModel.document.DocumentStatus = false;
            documentModel.document.DocumentPdfUrl = "";
            documentModel.document.DocumentVerificationCode = GenerateVerificationCode();







            return View();
        }

        #endregion
    }
}