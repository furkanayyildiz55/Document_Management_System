using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using DocumentManagementSystem.Models;
using DataAccessLayer.Concrete.Repositories;

namespace DocumentManagementSystem.Controllers
{
    [Authorize(Roles="1")]
    public class DocumentTypeController : Controller
    {
        DocumentTypeManager documentTypeManager = new DocumentTypeManager(new EfDocumentTypeDal());
        AdminManager adminManager = new AdminManager(new EfAdminDal());
        DocumentTypeSignatureManager DocumentTypeSignatureManager = new DocumentTypeSignatureManager(new EfDocumentTypeSignatureDal());


        #region DocumentTypeList

        public ActionResult DocumentTypeList()
        {
            var documentTypeList = documentTypeManager.GetList();
            return View(documentTypeList);
        }

        #endregion

        //TODO : Aynı documenttype varmı kontrolü
        #region AddDocumentType
 
        private List<SelectListItem> selectListItems()
        {
            return (from x in adminManager.GetListTopLevelAdmin( )
                    select new SelectListItem
                    {
                        Text = $"{x.AdminName} {x.AdminSurmane} - {x.AdminJob}",
                        Value = x.AdminID.ToString()
                    }).ToList();
        }


        [HttpGet]
        public ActionResult AddDocumentType()
        {
            DocumentTypeModel documentTypeModel = new DocumentTypeModel();
            documentTypeModel.selectAdminItems = selectListItems();
            return View(documentTypeModel);
        }


        [HttpPost]
        public ActionResult AddDocumentType(DocumentTypeModel documentTypeModel, HttpPostedFileBase file)
        {
            documentTypeModel.selectAdminItems = selectListItems();
            documentTypeModel.documentType.DocumentTypeCreateDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            if (documentTypeModel.AdminIds!= null)
            {
                documentTypeModel.documentType.DocumentTypeNumSignature = documentTypeModel.AdminIds.Length;
            }

            DocumentTypeValidator documentTypeValidator = new DocumentTypeValidator();
            ValidationResult result = documentTypeValidator.Validate(documentTypeModel.documentType);



            try
            {
                if (result.IsValid)
                {

                    if (FileUploadControl())
                    {
                       int RegisterDocumentTypeId= documentTypeManager.DocumentTypeAdd(documentTypeModel.documentType);
                        
                        
                        foreach (var Adminid in documentTypeModel.AdminIds)
                        {
                            int align = 1;
                            DocumentTypeSignature documentTypeSignature = new DocumentTypeSignature();
                            documentTypeSignature.AdminID = Adminid;
                            documentTypeSignature.DocumentTypeSignatureAlign = align.ToString();
                            documentTypeSignature.DocumentTypeID = RegisterDocumentTypeId;
                            DocumentTypeSignatureManager.DocumentTypeSignatureAdd(documentTypeSignature);
                            align++;
                        }



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
                return View(documentTypeModel);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = "Bilinmeyen hata, lütfen hatayı yetkililere bildiriniz ! : " + e.Message;
                return View(documentTypeModel);
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
                            documentTypeModel.documentType.DocumentTypeBacgroundImage= path;
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
        public ActionResult UpdateDocumentType(DocumentType documentType, HttpPostedFileBase file)
        {
            DocumentTypeValidator documentTypeValidator = new DocumentTypeValidator();
            ValidationResult result = documentTypeValidator.Validate(documentType);

            try
            {
                if (result.IsValid)
                {
                    int fileUploadReturn = FileUploadControl();
                    if (fileUploadReturn == 1 || fileUploadReturn == 0 )
                    {
                        documentTypeManager.DocumentTypeUpdate(documentType);
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
                ViewBag.RecordStatus = false;
                ViewBag.UploadError = "Bilinmeyen hata, lütfen hatayı yetkililere bildiriniz ! : " + e.Message;
                return View();
            }



            int FileUploadControl()
            {
                // true => dosya yüklemesi tamamlandı
                // false => dosya yüklemesi tamamlandmadı 


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
                            documentType.DocumentTypeBacgroundImage = path;
                            return 1;

                        }
                        else
                        {
                            ViewBag.UploadError = "Dosya Boyutu 5 Mb dan küçük olmalı !";
                            return -1;
                        }

                    }
                    else
                    {
                        ViewBag.UploadError = "Lütfen PNG veya JPG biçiminde resim yükleyin !";
                        return -1;
                    }
                }
                else
                {
                    //arka plan yüklemeden devam etmek için
                    return 0;
                }
            }
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


