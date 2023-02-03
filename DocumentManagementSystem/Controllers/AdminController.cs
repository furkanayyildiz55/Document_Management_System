using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System;
using System.IO;
using System.Web.Mvc;

namespace DocumentManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        AdminManager adminManager = new AdminManager(new EfAdminDal());



        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddAdmin()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddAdmin(Admin admin)
        {
            admin.AdminStatus = true;

            AdminValidator adminValidator = new AdminValidator();
            ValidationResult result = adminValidator.Validate(admin);

            if(Request.Files.Count > 0)
            {
                string fileName = Path.GetFileName(Request.Files[0].FileName);
                string uzanti = Path.GetExtension(Request.Files[0].FileName);
                string guid = Guid.NewGuid().ToString();
                //string path = "~/Image/signature/" + fileName + uzanti;
                string path = $"/Image/signature/{fileName}_{guid}{uzanti}" ;
                
                //servera kaydetme
                Request.Files[0].SaveAs(Server.MapPath("~"+path));

                //Veritabanına ekleme 
                admin.AdminSignatureImage = path;

            }

            if (result.IsValid)
            {
                adminManager.AdminAdd(admin);
            }
            else
            {
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();
        }


    }
}