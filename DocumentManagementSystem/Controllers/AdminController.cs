using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

using BCrypt;

namespace DocumentManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        AdminManager adminManager = new AdminManager(new EfAdminDal());

        // GET: Admin
        //[Authorize]  //kullanıcı girişi olmadan girilemeyeceği anlmını taşır

        #region AddAdmin

        [HttpGet ]
        public ActionResult AddAdmin()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddAdmin(Admin admin, HttpPostedFileBase file)
        {
            AdminValidator adminValidator = new AdminValidator();
            ValidationResult result = adminValidator.Validate(admin);

            //// Şifre doğrulama
            //string userInput = "mypassword"; // Kullanıcının girdiği şifre
            //bool isPasswordValid = BCrypt.Net.BCrypt.Verify(userInput, hashedPassword); // Hashlenmiş şifreyi doğrula
            try
            {
                if (result.IsValid) //form validasyonu sağlanıyorsa
                {
                    admin.AdminStatus = true;
                    // Şifre hashleme
                    string salt = BCrypt.Net.BCrypt.GenerateSalt(); // Salt oluştur
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(admin.AdminPassword, salt); // Şifreyi hashle
                    admin.AdminPassword = hashedPassword;

                    if (admin.AdminAuthorization)  //true ise kesinlikle görsel yüklemesi olacak
                    {
                        if (FileUploadControl())
                        {
                            adminManager.AdminAdd(admin);
                            ViewBag.RecordStatus = true;
                        }
                        else  // görsel yüklemesi olmadan kayıt yapılabilecek
                        {
                            ViewBag.RecordStatus = false;
                        }
                    }
                    else  //false durumunda görsel yüklemesi olmasada kayıt edilebilecek
                    {
                        adminManager.AdminAdd(admin);
                        ViewBag.RecordStatus = true;
                    }

                }
                else //validasyon sağlanmıyor ise
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
                ViewBag.RecordStatus = false;
                ViewBag.UploadError = "Bilinmeyen hata, lütfen hatayı yetkililere bildiriniz ! : "+e.Message;
                return View();
            }

            bool FileUploadControl()
            {
                // true => dosya yüklemesi tamamlandı
                // false => dosya yüklemesi tamamlandmadı 
                //code formar ctrl+k+d

                ViewBag.UploadError = "UPLOAD ERROR ";


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
                            ViewBag.UploadError = "Dosya Boyutu 5 Mb dan küçük olmalı !";
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
        #endregion

        #region AdminUpdate

        [HttpGet]
        public ActionResult AdminUpdate(int? id)
        {

            if (id != null && id >= 0)
            {
                var adminValue = adminManager.GetAdmin((int)id);
                return View(adminValue);
            }
            else
            {
                return RedirectToAction("AdminList");
            }
        }

        [HttpPost]
        public ActionResult AdminUpdate(Admin admin, HttpPostedFileBase file)
        {
            AdminValidator adminValidator = new AdminValidator();
            ValidationResult result = adminValidator.Validate(admin);

            try
            {
                if (result.IsValid) //form validasyonu sağlanıyorsa
                {
                    if (admin.AdminAuthorization)  //true ise kesinlikle görsel yüklemesi olacak
                    {
                        if (FileUploadControl())
                        {
                            adminManager.AdminUpdate(admin);
                            ViewBag.RecordStatus = true;
                        }
                        else  // görsel yüklemesi olmadan kayıt yapılabilecek
                        {
                            ViewBag.RecordStatus = false;
                        }
                    }
                    else  //false durumunda görsel yüklemesi olmasada kayıt edilebilecek
                    {
                        adminManager.AdminUpdate(admin);
                        ViewBag.RecordStatus = true;
                    }

                }
                else //validasyon sağlanmıyor ise
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
                ViewBag.RecordStatus = false;
                ViewBag.UploadError = "Bilinmeyen hata, lütfen hatayı yetkililere bildiriniz ! : " + e.Message;
                return View();
            }

            bool FileUploadControl()
            {
                // true => dosya yüklemesi tamamlandı
                // false => dosya yüklemesi tamamlandmadı 
                //code formar ctrl+k+d

                ViewBag.UploadError = "UPLOAD ERROR ";


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
                            ViewBag.UploadError = "Dosya Boyutu 5 Mb dan küçük olmalı !";
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

        #endregion

        #region AdminList

        public ActionResult AdminList()
        {
            var adminList = adminManager.GetList();
            return View(adminList);
        }

        #endregion

        #region Methods

        public ActionResult ChangeAdminStatus(int id)
        {
            if (id < 0)
            {
                return RedirectToAction("AdminList");
            }
            else
            {
                Admin admin = adminManager.GetAdmin(id);
                adminManager.AdminDelete(admin);
                return RedirectToAction("AdminList");
            }
        }

        #endregion

    }
}