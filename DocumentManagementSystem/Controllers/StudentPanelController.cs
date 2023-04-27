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
    public class StudentPanelController : Controller
    {
        DocumentManager documentManager = new DocumentManager(new EfDocumentDal());

        [Authorize]
        public ActionResult Documents()
        {
            int StudentID = (int)Session["UserID"];

            List<Document> documentList = documentManager.GetListWithStudentID(StudentID);

            return View(documentList);
        }

        //TODO : PDF olarak görüntüleme sayfası açılacak
        //TODO : PDF indir tusuna indirme işlemi yapılacak
        public ActionResult DocumentView (int? id)
        {
            return View();
        }

    }
}