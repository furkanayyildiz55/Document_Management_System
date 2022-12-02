using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentManagementSystem.Controllers
{
    public class DocumentTypeController : Controller
    {
        DocumentTypeManager documentTypeManager = new DocumentTypeManager(new  EfDocumentTypeDal());


        // GET: DocumentType
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetDocumentTypeList()
        {
            var documentTypeList = documentTypeManager.GetList();
            return View(documentTypeList);
        }

        //Sadece sayafayı yükler
        [HttpGet]
        public ActionResult AddDocumentType()
        {
            return View();
        }


        //Sayfadan veri gönderimi olduğunda butona tıklandığında çalışır
        [HttpPost]
        public ActionResult AddDocumentType(DocumentType documentType )
        {
            documentType.DocumentTypeStatus = true;
            documentType.DocumentCreateDate= DateTime.Parse(DateTime.Now.ToShortDateString());

            DocumentTypeValidator documentTypeValidator = new DocumentTypeValidator();
            ValidationResult result = documentTypeValidator.Validate(documentType);

            if (result.IsValid)
            {
                documentTypeManager.DocumentTypeAdd(documentType);
                return RedirectToAction("GetDocumentTypeList");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult EditDocumentType() 
        {
            return View();
        }


        [HttpPost]
        public ActionResult EditDocumentType( DocumentType documentType)
        {
            return View();
        }
    }
}