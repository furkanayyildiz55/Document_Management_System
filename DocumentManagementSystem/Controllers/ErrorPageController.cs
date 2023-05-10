using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentManagementSystem.Controllers
{
    public class ErrorPageController : Controller
    {


        public ActionResult Page400()
        {
            Response.StatusCode = 400;
            Response.TrySkipIisCustomErrors = true;
            ViewBag.RandomNumber = RandomNumber();
            ViewBag.ErrorCode = "400";
            ViewBag.ErrorMessage = "Sunucu Taraflı Hata. Bunun İçin Üzgünüz !";
            return View();
        }

        public ActionResult Page401()
        {
            Response.StatusCode = 401;
            Response.TrySkipIisCustomErrors = true;
            ViewBag.RandomNumber = RandomNumber();
            ViewBag.ErrorCode = "401";
            ViewBag.ErrorMessage = "Yetkisiz Erişim !";
            return View();
        }

        public ActionResult Page404()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
            ViewBag.RandomNumber = RandomNumber();
            ViewBag.ErrorCode = "404";
            ViewBag.ErrorMessage = "Aranılan Sayfa Bulunamadı !";
            return View();
        }

        public ActionResult Page500()
        {
            Response.StatusCode = 500;
            Response.TrySkipIisCustomErrors = true;
            ViewBag.RandomNumber = RandomNumber();
            ViewBag.ErrorCode = "500";
            ViewBag.ErrorMessage = "Sunucu Taraflı Hata. Bunun İçin Üzgünüz !";
            return View();
        }

        public ActionResult Page503()
        {
            Response.StatusCode = 503;
            Response.TrySkipIisCustomErrors = true;
            ViewBag.RandomNumber = RandomNumber();
            ViewBag.ErrorCode = "503";
            ViewBag.ErrorMessage = "Sunucu Geçici Olarak Erişilemez Durumda !";
            return View();
        }

        private int RandomNumber()
        {
            Random random = new Random();
            int randomNumber = random.Next(1, 6);
            return randomNumber;
        }
    }
}