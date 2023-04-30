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


    #region DocumentVerification

    public class DocumentVerificationController : ApiController
    {
        public HttpResponseMessage Get(string Code)
        {
            DocumentManager documentManager = new DocumentManager(new EfDocumentDal());
            DocumentVerificationResponse documentVerificationResponse = new DocumentVerificationResponse();

            try
            {
                Document Document = documentManager.GetDocumentWithVerificationCode(Code);
                DocumentModel DocumentModel = new DocumentModel();

                if(Document != null)
                {
                    DocumentModel.DocumentID = Document.StudentID.ToString();
                    DocumentModel.DocumentName = Document.DocumentType.DocumentTypeName;
                    DocumentModel.DocumentPdfUrl = Document.DocumentPdfUrl;
                    DocumentModel.DocumentCreateDate = Document.DocumentCreateDate;
                    DocumentModel.DocumentStatus = Document.DocumentStatus;

                    documentVerificationResponse.Verification = true;
                    documentVerificationResponse.VerificationMessage = "Doğrulanmış belge";
                    documentVerificationResponse.Document = DocumentModel;
                }
                else
                {
                    documentVerificationResponse.Verification = false;
                    documentVerificationResponse.VerificationMessage = "Böyle bir belge bulunmamaktadır !";
                    documentVerificationResponse.Document = null;
                }
                return Request.CreateResponse(HttpStatusCode.OK, documentVerificationResponse);

            }
            catch (Exception)
            {
                documentVerificationResponse.Verification = false;
                documentVerificationResponse.VerificationMessage = "Üzgünüz, bir hata oluştu.";
                documentVerificationResponse.Document = null;
                return Request.CreateResponse(HttpStatusCode.OK, documentVerificationResponse);

            }
        }

        public class DocumentVerificationResponse
        {

            public bool Verification { get; set; }
            public string VerificationMessage { get; set; }
            public DocumentModel Document { get; set; }
        }

        public class DocumentModel
        {
            public string DocumentID { get; set; }
            public string DocumentName { get; set; }
            public string DocumentPdfUrl { get; set; }
            public DateTime DocumentCreateDate { get; set; }
            public bool DocumentStatus { get; set; }
        }

    }

    #endregion

    #region SignIn

    public class SignInController : ApiController
    {
        StudentManager studentManager = new StudentManager(new EfStudentDal());

        public HttpResponseMessage Post(SignInModel signInModel)
        {
            SignInRespose signInRespose = new SignInRespose();

            try
            {
                Student student = studentManager.GetStudentWithMail(signInModel.Email);

                if (student == null)
                {
                    signInRespose.SignIn = false;
                    signInRespose.SignInError = "Kullanıcı bulunamadı !";
                }
                else
                {
                    // Şifre doğrulama
                    bool isPasswordValid = BCrypt.Net.BCrypt.Verify(signInModel.Password, student.StudentPassword); // Hashlenmiş şifreyi doğrula
                    if (isPasswordValid)
                    {
                        //giriş yapılabilir
                        student.StudentPassword = "";
                        signInRespose.SignIn = true;
                        signInRespose.SignInError = "";
                        signInRespose.Student = student;

                    }
                    else
                    {
                        //şifre yanlış
                        signInRespose.SignIn = false;
                        signInRespose.SignInError = "Şifre hatalı !";

                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, signInRespose);
            }
            catch (Exception)
            {
                signInRespose.SignIn = false;
                signInRespose.SignInError = "Üzgünüz, bir hata oluştu";
                return Request.CreateResponse(HttpStatusCode.OK, signInRespose);
            }

        }
        public class SignInModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class SignInRespose
        {
            public bool SignIn { get; set; }
            public string SignInError { get; set; }
            public Student Student { get; set; }

        }
    }

    #endregion

    #region DocumentList

    public class DocumentApiController : ApiController
    {
        DocumentManager DocumentManager = new DocumentManager(new EfDocumentDal());

        public HttpResponseMessage Get(string StudentID)
        {
            DocumentRespose documentRespose = new DocumentRespose();

            try
            {
                List<Document> documentList = DocumentManager.GetListWithStudentID(int.Parse(StudentID));

                if (documentList == null )
                {
                    documentRespose.Status= false;
                    documentRespose.Message= "Tanımlanmış belge yok";
                }
                else
                {
                    documentRespose.Status = true;
                    documentRespose.Message = "Tanımlanan belgeler listelendi";
                    foreach (var document  in documentList)
                    {
                        DocumentModel documentModel = new DocumentModel(document);
                        documentRespose.DocumentModel.Add(documentModel);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, documentRespose);
            }
            catch (Exception)
            {
                documentRespose.Status = false;
                documentRespose.Message = "Üzgünüz, bir hata oluştu";
                return Request.CreateResponse(HttpStatusCode.OK, documentRespose);
            }

        }

        public class DocumentRespose
        {
            public bool Status { get; set; }
            public string Message { get; set; }
            public List<DocumentModel> DocumentModel{ get; set; }
        }

        public class DocumentModel
        {
            public DocumentModel(Document document)
            {
                DocumentID = document.DocumentID.ToString();
                DocumentName = document.DocumentType.DocumentTypeName;
                DocumentPdfUrl = document.DocumentPdfUrl;
                DocumentCreateDate = document.DocumentCreateDate;
                DocumentVerificationCode = document.DocumentVerificationCode;
                DocumentStatus = document.DocumentStatus;
            }

            public string DocumentID { get; set; }
            public string DocumentName { get; set; }
            public string DocumentPdfUrl { get; set; }
            public DateTime DocumentCreateDate { get; set; }
            public string DocumentVerificationCode { get; set; }
            public bool DocumentStatus { get; set; }
        }

    }

    #endregion






    public class DemoController : ApiController
    {
        public string Get()
        {
            return "Welcome To Web API";
        }
        //https://localhost:44371/api/Demo?id=1&name=steve
        public List<string> Get(int id , string name)
        {
            return new List<string> {
                "Data1",
                "Data2",
                id.ToString(),
                name.ToString()
            };
        }

    }





}
