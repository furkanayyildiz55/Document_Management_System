using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace DocumentManagementSystem.Controllers
{
    [Authorize(Roles = "0,1")]
    public class StudentController : Controller
    {
        StudentManager studentManager = new StudentManager(new EfStudentDal());

        #region StudentList
        public ActionResult StudentList()
        {
            var studentList = studentManager.GetList();
            return View(studentList);
        }
        #endregion

        //TODO :  aynı student varmı kontrolü
        #region StudentAdd

        [HttpGet]
        public ActionResult StudentAdd()
        {
            return View();
        }


        [HttpPost]
        public ActionResult StudentAdd(Student student)
        {
            StudentValidator studentValidator = new StudentValidator(student);
            ValidationResult result = studentValidator.Validate(student);

            try
            {
                if (result.IsValid)
                {
                    Student DbStudentNo = null;
                    if (student.StudentUniversityRegistered)
                    {
                        DbStudentNo = studentManager.GetStudentWihtNumber(student.StudentNo);
                    }
                   
                    Student DbStudentMail = studentManager.GetStudentWithMail(student.StudentMail);

                    if(DbStudentNo == null && DbStudentMail == null)
                    {
                        string salt = BCrypt.Net.BCrypt.GenerateSalt(); // Salt oluştur
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(student.StudentPassword, salt); // Şifreyi hashle
                        student.StudentPassword= hashedPassword;

                        studentManager.StudentAdd(student);
                        ViewBag.RecordStatus = true;
                    }
                    else
                    {
                        if (student.StudentUniversityRegistered)
                        {
                            if (DbStudentNo != null) ModelState.AddModelError("StudentNo", "Bu numara ile kayıtlı öğrenci bulunmaktadır");
                        }

                        if (DbStudentMail != null) ModelState.AddModelError("StudentMail", "Bu mail ile kayıtlı öğrenci bulunmaktadır");
                    }


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
            catch (Exception e)
            {
                ViewBag.ErrorMessage= "Bilinmeyen hata, lütfen hatayı yetkililere bildiriniz ! : " + e.Message;
                return View();
            }
        }

        #endregion



    }
}