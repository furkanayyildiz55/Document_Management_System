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
using System.Web.Security;
using BCrypt;


namespace DocumentManagementSystem.Controllers
{
    public class HomeController : Controller
    {

        DocumentManager documentManager = new DocumentManager(new EfDocumentDal());
        AdminManager adminManager = new AdminManager(new EfAdminDal());
        StudentManager studentManager = new StudentManager(new EfStudentDal());


        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(SignInModel signInModel)
        {
            string SignInError = "";

            if (signInModel.UserIsAdmin) //admin
            {
                Admin admin = adminManager.GetAdminWithMail(signInModel.UserMail);
                if(admin == null)
                {
                    SignInError = "Kullanıcı bulunamadı !";
                    ModelState.AddModelError("SignInError", SignInError);
                }
                else
                {
                    // Şifre doğrulama
                    bool isPasswordValid = BCrypt.Net.BCrypt.Verify(signInModel.UserPassword, admin.AdminPassword); // Hashlenmiş şifreyi doğrula
                    if (isPasswordValid)
                    {
                        //giriş yapılabilir
                        FormsAuthentication.SetAuthCookie(admin.AdminName,false);
                        Session["UserName"] = admin.AdminName + admin.AdminSurmane;
                        Session["UserID"] = admin.AdminID;
                        Session["USerIsAdmin"] = true;
                        return RedirectToAction("AddAdmin", "Admin");
                    }
                    else
                    {
                        SignInError = "Şifre hatalı !";
                        ModelState.AddModelError("SignInError", SignInError);
                    }
                }

            }
            else if (!signInModel.UserIsAdmin) //student
            {
                Student student = studentManager.GetStudentWithMail(signInModel.UserMail);
                if (student == null)
                {
                    SignInError = "Kullanıcı bulunamadı !";
                    ModelState.AddModelError("SignInError", SignInError);
                }
                else
                {
                    // Şifre doğrulama
                    bool isPasswordValid = BCrypt.Net.BCrypt.Verify(signInModel.UserPassword, student.StudentPassword); // Hashlenmiş şifreyi doğrula
                    if (isPasswordValid)
                    {
                        //giriş yapılabilir
                        FormsAuthentication.SetAuthCookie(student.StudentName, false);
                        Session["UserName"] = student.StudentName + student.StudentSurname;
                        Session["UserID"] = student.StudentID;
                        Session["UserIsAdmin"] = false;
                        return RedirectToAction("AddAdmin", "Admin");
                    }
                    else
                    {
                        SignInError = "Şifre hatalı !";
                        ModelState.AddModelError("SignInError" , SignInError);
                    }
                }

            }
            else
            {
                SignInError = "Kullanıcı türü seçin !";
                ModelState.AddModelError("SignInError", SignInError);
            }

            return View(signInModel);


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