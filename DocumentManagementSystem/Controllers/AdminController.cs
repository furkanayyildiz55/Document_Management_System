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
        public ActionResult AddAdmin(Admin admin , HttpPostedFileBase file)
        {
            admin.AdminStatus = true;

            AdminValidator adminValidator = new AdminValidator();
            ValidationResult result = adminValidator.Validate(admin);


            if (result.IsValid)
            {
                FileUploadControl();
                adminManager.AdminAdd(admin);
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();



            void FileUploadControl() 
            {     

                if (admin.AdminAuthorization == true)  //Eğer admin üst düzey yetkili ise imzası da kesinlikle olacak !
                {
                    if (file != null) 
                    {
                        string FileExt = Path.GetExtension(file.FileName);
                        int FileLength = file.ContentLength;
                        if (FileExt == ".png")
                        {
                            ViewBag.ExtensionError = false;

                            if (FileLength <= 5242880)  //5 Mb dan küçük
                            {
                                ViewBag.LenghtError = false;
                                string guid = Guid.NewGuid().ToString();
                                string path = "/image/signature/" + guid + FileExt;

                                //Sunucu kaydetme
                                file.SaveAs(Server.MapPath("~" + path));
                                //Veritabanı kaydetme
                                admin.AdminSignatureImage = path;



                            }
                            else
                            {
                                ViewBag.LenghtError = true;
                            }

                        }
                        else
                        {
                            ViewBag.ExtensionError = true;
                        }

                    }
                } 
            
            }





            /*
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
            */



        }


    }
}