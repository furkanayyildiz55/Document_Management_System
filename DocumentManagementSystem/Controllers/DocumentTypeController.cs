using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
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

        public ActionResult AddDocumentType(DocumentType documentType )
        {
            documentTypeManager.DocumentTypeAdd(documentType);
            return RedirectToAction("GetDocumentTypeList");
        }
    }
}