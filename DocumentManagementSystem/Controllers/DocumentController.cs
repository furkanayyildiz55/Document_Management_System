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
        // GET: Document
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddDocument()
        {

            return View();
        }

        [HttpPost]
        public ActionResult AddDocument(Document document )
        {

            return View();
        }
    }
}