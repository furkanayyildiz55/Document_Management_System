using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DocumentManagementSystem.Controllers
{
    public class ApiDocumentVerificationController : ApiController
    {
        [HttpGet]
        public IEnumerable<DocumentVerificationApiModel> DocumentVerification()
        {
            DocumentManager documentManager = new DocumentManager(new EfDocumentDal());
            Document Document = documentManager.GetDocumentWithVerificationCode("BB48AB4E9743");

            DocumentVerificationApiModel apimodel = new DocumentVerificationApiModel(Document.DocumentType.DocumentTypeName, Document.DocumentCreateDate, Document.DocumentID.ToString(), Document.Student.StudentName);
            List<DocumentVerificationApiModel> documentVerification = new List<DocumentVerificationApiModel>();
            documentVerification.Add(apimodel);

            return documentVerification;

        }
    }

    public class  DocumentVerificationApiModel
    {
        public DocumentVerificationApiModel(string documentName, DateTime documentCreateDate, string documentID, string studentName)
        {
            DocumentName = documentName;
            DocumentCreateDate = documentCreateDate;
            DocumentID = documentID;
            StudentName = studentName;
        }

        public string DocumentName { get; set; }
        public DateTime DocumentCreateDate{ get; set; }
        public string DocumentID { get; set; }
        public string StudentName { get; set; }


    }


}
