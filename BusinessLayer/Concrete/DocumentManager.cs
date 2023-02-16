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


        public void DocumentAdd(Document document)
        {
            _document.Add(document);
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
    }
}
