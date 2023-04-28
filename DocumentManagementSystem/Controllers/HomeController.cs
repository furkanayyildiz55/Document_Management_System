using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using DocumentManagementSystem.Models;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Newtonsoft.Json;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;



namespace DocumentManagementSystem.Controllers
{
    public class HomeController : Controller
    {

        DocumentManager documentManager = new DocumentManager(new EfDocumentDal());
        AdminManager adminManager = new AdminManager(new EfAdminDal());
        StudentManager studentManager = new StudentManager(new EfStudentDal());

        #region SignIn

        public ActionResult SignIn()
        {
            //varolan session varmı kontrol et
            if (Session["UserID"] != null)
            {
                bool UserIsAdmin = (bool)Session["UserIsAdmin"];
                if (UserIsAdmin)
                {
                    int userID = (int)Session["UserID"];
                    Admin admin = adminManager.GetAdmin(userID);
                    if (admin != null)
                    {
                        return RedirectToAction("AddDocument", "Document");

                    }
                }
                else
                {
                    return RedirectToAction("Documents", "StudentPanel");
                }


                // Kullanıcı oturumu zaten açılmışsa, istenen sayfaya yönlendir
            }
            else
            {
                // Kullanıcı oturumu açılmamışsa, giriş sayfasını göster
                return View();
            }
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
                        FormsAuthentication.SetAuthCookie(admin.AdminID.ToString(),false);
                        Session["UserName"] = admin.AdminName+" " + admin.AdminSurmane;
                        Session["UserID"] = admin.AdminID;
                        Session["UserIsAdmin"] = true;
                        return RedirectToAction("AddDocument", "Document");
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
                        FormsAuthentication.SetAuthCookie("student"+student.StudentID.ToString(), false);
                        Session["UserName"] = student.StudentName + " "+ student.StudentSurname;
                        Session["UserID"] = student.StudentID;
                        Session["UserIsAdmin"] = false;
                        return RedirectToAction("Documents", "StudentPanel");
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

        #endregion

        #region SignOut

        public ActionResult SignOut()
        {
            try
            {
                FormsAuthentication.SignOut();
                Session.Abandon();
            }
            catch (System.Exception)
            {
            }
            return RedirectToAction("SignIn", "Home");


        }

        #endregion

        #region Home

        public ActionResult Home()
        {
            DocumentVerificationModel documentVerificationModel = new DocumentVerificationModel();
            documentVerificationModel.isPostMethod = false;
            return View( documentVerificationModel);
        }

        [HttpPost]
        public ActionResult Home(DocumentVerificationModel documentVerificationModel)
        {
            documentVerificationModel.isPostMethod = true;


            var response = Request["g-recaptcha-response"];
            const string secret = "6LeTsZslAAAAAJc8ieouPqL-FfDiy5WtrUwBbkQH";

            var client = new WebClient();
            var reply =
                client.DownloadString(
                    string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);
            documentVerificationModel.captchaStatus = captchaResponse.Success;


            if (captchaResponse.Success)
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
                    //eğer js doğrulaması kötü niyetle atlanırsa null dönecektir null dönme durumu viev de işlenmiştir
                    //foreach (var item in result.Errors)
                    //{
                    //    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    //}
                }
                return View(documentVerificationModel);
            }
            else
            {
                return View(documentVerificationModel);
            }


        }

        #endregion



    }
}