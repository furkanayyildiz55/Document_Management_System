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
    public class StudentPanelController : Controller
    {
        DocumentManager documentManager = new DocumentManager(new EfDocumentDal());
        StudentManager studentManager = new StudentManager(new EfStudentDal());


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


        [HttpGet, Authorize]
        public ActionResult StudentProfile()
        {

            int StudentID = (int)Session["UserID"];

            Student student = studentManager.GetStudent(StudentID);
            student.StudentPassword = null;
            return View(student);
        }

        [HttpPost, Authorize]
        public ActionResult StudentProfile(Student newStudent)
        {
            Student student = studentManager.GetStudent(newStudent.StudentID);

            if (newStudent.StudentPassword != null)
            {
                student.StudentPassword = newStudent.StudentPassword;
            }

            StudentValidator studentValidator = new StudentValidator(student);
            ValidationResult result = studentValidator.Validate(student);

            try
            {
                if (result.IsValid) //form validasyonu sağlanıyorsa
                {
                    if (newStudent.StudentPassword != null)
                    {
                        // Şifre hashleme
                        string salt = BCrypt.Net.BCrypt.GenerateSalt(); // Salt oluştur
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(student.StudentPassword, salt); // Şifreyi hashle
                        student.StudentPassword = hashedPassword;
                    }

                    studentManager.StudentUpdate(student);
                    ViewBag.RecordStatus = true;
                    ViewBag.ErrorMessage = "Güncelleme gerçekleşti";
                }
                else //validasyon sağlanmıyor ise
                {
                    ViewBag.RecordStatus = false;
                    ViewBag.ErrorMessage = "Güncelleme gerçekleşmedi";

                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
                return View(student);
            }
            catch (Exception e)
            {
                ViewBag.RecordStatus = false;
                ViewBag.ErrorMessage = "Bilinmeyen hata, lütfen hatayı yetkililere bildiriniz ! : " + e.Message;
                return View(student);
            }

        }

    }
}