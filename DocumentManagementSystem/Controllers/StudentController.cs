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
                    studentManager.StudentAdd(student);
                    ViewBag.RecordStatus = true;
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