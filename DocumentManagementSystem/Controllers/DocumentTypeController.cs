﻿using BusinessLayer.Concrete;
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
    public class DocumentTypeController : Controller
    {
        DocumentTypeManager documentTypeManager = new DocumentTypeManager(new EfDocumentTypeDal());


        // GET: DocumentType
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DocumentTypeList()
        {
            var documentTypeList = documentTypeManager.GetList();
            return View(documentTypeList);
        }




        #region AddDocumentType

        //Sadece sayafayı yükler
        [HttpGet]
        public ActionResult AddDocumentType()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddDocumentType(DocumentType documentType, HttpPostedFileBase file)
        {
            documentType.DocumentCreateDate = DateTime.Parse(DateTime.Now.ToShortDateString());


            DocumentTypeValidator documentTypeValidator = new DocumentTypeValidator();
            ValidationResult result = documentTypeValidator.Validate(documentType);

            try
            {
                if (result.IsValid)
                {
                    if (FileUploadControl())
                    {
                        documentTypeManager.DocumentTypeAdd(documentType);
                        ViewBag.RecordStatus = true;
                    }
                    else
                    {
                        ViewBag.RecordStatus = false;
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
                ViewBag.ErrorMessage = "Bilinmeyen hata, lütfen hatayı yetkililere bildiriniz ! : " + e.Message;
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
                    if (FileExt == ".png" || FileExt == ".jpg")
                    {
                        ViewBag.ExtensionError = false;
                        if (FileLength <= 5242880)  //5 Mb dan küçük
                        {
                            string guid = Guid.NewGuid().ToString();
                            string path = "/image/documentTypeImage/" + guid + FileExt;

                            //Sunucu kaydetme
                            file.SaveAs(Server.MapPath("~" + path));
                            //Veritabanı kaydetme
                            documentType.DocumentTypeBacgroundImage= path;
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
                        ViewBag.UploadError = "Lütfen PNG veya JPG biçiminde resim yükleyin !";
                        return false;
                    }
                }
                else
                {
                    ViewBag.UploadError = "Dosya türü için arkaplan görüntüsü yükleyiniz !";
                    return false;

                }
            }
        }


        #endregion


        #region UpdateDocumentType

        [HttpGet]
        public ActionResult UpdateDocumentType(int? id)
        {
            if (id != null && id >= 0)
            {
                var documentValue = documentTypeManager.GetDocumentType((int)id);
                return View(documentValue);
            }
            else
            {
                return RedirectToAction("AdminList");
            }
        }


        [HttpPost]
        public ActionResult UpdateDocumentType(DocumentType documentType)
        {
            return View();
        }

        #endregion



        [HttpGet]
        public ActionResult StatusChange(int id)
        {
            var documentType = documentTypeManager.GetDocumentType(id);
            documentType.DocumentTypeStatus = !documentType.DocumentTypeStatus;
            documentTypeManager.DocumentTypeUpdate(documentType);
            return RedirectToAction("GetDocumentTypeList");
        }
    }
}


