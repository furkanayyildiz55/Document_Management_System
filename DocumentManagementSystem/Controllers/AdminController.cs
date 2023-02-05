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
        public ActionResult AddAdmin(Admin admin, HttpPostedFileBase file)
        {
            admin.AdminStatus = true;

            AdminValidator adminValidator = new AdminValidator();
            ValidationResult result = adminValidator.Validate(admin);


            if (result.IsValid)
            {
                if(admin.AdminAuthorization)
                {
                    if (FileUploadControl())
                    {
                        ViewBag.UploadStatus = true;
                        FileUploadControl();
                        adminManager.AdminAdd(admin);
                    }
                    {
                        ViewBag.UploadStatus = false;
                        //
                    }
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



            bool FileUploadControl()
            {
                // true => dosya yüklemesi tamamlandı
                // false => dosya yüklemesi tamamlandmadı 
                //code formar ctrl+k+d

                if (file != null)
                {
                    string FileExt = Path.GetExtension(file.FileName);
                    int FileLength = file.ContentLength;
                    if (FileExt == ".png")
                    {
                        ViewBag.ExtensionError = false;
                        if (FileLength <= 5242880)  //5 Mb dan küçük
                        {
                            string guid = Guid.NewGuid().ToString();
                            string path = "/image/signature/" + guid + FileExt;

                            //Sunucu kaydetme
                            file.SaveAs(Server.MapPath("~" + path));
                            //Veritabanı kaydetme
                            admin.AdminSignatureImage = path;
                            return true;

                        }
                        else
                        {
                            ViewBag.UploadtError = "Dosya Boyutu 5 Mb dan küçük olmalı !";
                            return false;
                        }

                    }
                    else
                    {
                        ViewBag.UploadError = "Lütfen PNG biçiminde resim yükleyin !";
                        return false;
                    }
                }
                else
                {
                    ViewBag.UploadError = "Üst düzey yetkililer imza yüklemek zorundadır !";
                    return false;

                }
            }
        }


    }
}