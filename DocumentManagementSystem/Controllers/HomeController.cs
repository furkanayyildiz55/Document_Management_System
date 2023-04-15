using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using DocumentManagementSystem.Models;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentManagementSystem.Controllers
{
    public class HomeController : Controller
    {

        DocumentManager documentManager = new DocumentManager(new EfDocumentDal());


        public ActionResult SignIn()
        {
            return View();
        }

        public ActionResult Home()
        {
            DocumentVerificationModel documentVerificationModel = new DocumentVerificationModel();
            documentVerificationModel.isPostMethod = false;
            return View( documentVerificationModel);
        }

        [HttpPost]
        public ActionResult Home(DocumentVerificationModel documentVerificationModel)
        {
            DocumentVerificationValidator documentVerificationValidator = new DocumentVerificationValidator();
            Document ValidationDocument = new Document();
            ValidationDocument.DocumentVerificationCode = documentVerificationModel.VerificationCode;
            ValidationResult result = documentVerificationValidator.Validate(ValidationDocument);

            if (result.IsValid)
            {
                documentVerificationModel.document = documentManager.GetDocumentWithVerificationCode(documentVerificationModel.VerificationCode);
                documentVerificationModel.isPostMethod = true;
            }
            else
            {
                //foreach (var item in result.Errors)
                //{
                //    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                //}
            }
            documentVerificationModel.isPostMethod = true;
            return View(documentVerificationModel);
        }



    }
}