using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System.Collections.Generic;

namespace BusinessLayer.Concrete
{
    public class DocumentManager : IDocumentService
    {

        IDocumentDal _document;

        public DocumentManager(IDocumentDal document)
        {
            _document = document;
        }


        public int DocumentAdd(Document document)
        {
           return _document.Add(document);
        }

        public void DocumentDelete(Document document)
        {
            _document.Delete(document);
        }

        public void DocumentUpdate(Document document)
        {
            _document.Update(document);
        }

        public Document GetDocument(int documentID)
        {
            return _document.Get(x => x.DocumentID == documentID);
        }


        public List<Document> GetList()
        {
           return _document.List();
        }

        /// <summary>
        /// /////İNTERFACEYE GEÇİR
        /// </summary>
        /// <param name="StudentNo"></param>
        /// <returns></returns>
        public Document GetDocumentWithStudentNo(int StudentNo)
        {
            return _document.Get(x => x.Student.StudentNo == StudentNo.ToString());
        }

        public Document GetDocumentWithVerificationCode(string VerificationCode)
        {
            return _document.Get(x => x.DocumentVerificationCode == VerificationCode);
        }

        public List<Document> GetList(string StudentNo)
        {
            return _document.List(x => x.Student.StudentNo == StudentNo);
        }


    }
}
