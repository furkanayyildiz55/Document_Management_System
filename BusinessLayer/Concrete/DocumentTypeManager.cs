using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System.Collections.Generic;


namespace BusinessLayer.Concrete
{
    public class DocumentTypeManager : IDocumentTypeService
    {
        IDocumentTypeDal _documentType;


        public DocumentTypeManager(IDocumentTypeDal documentType)
        {
            _documentType = documentType;
        }

        public int DocumentTypeAdd(DocumentType documentType)
        {
           return _documentType.Add(documentType);
        }

        public void DocumentTypeDelete(DocumentType documentType)
        {
            _documentType.Delete(documentType);
        }

        public void DocumentTypeUpdate(DocumentType documentType)
        {
            _documentType.Update(documentType);
        }

        public List<DocumentType> GetList()
        {
          return  _documentType.List();
        }

        public DocumentType GetDocumentType(int DocumentTypeID)
        {
            return _documentType.Get(x => x.DocumentTypeID == DocumentTypeID);
        }

        public List<DocumentType> GetListActiveDocument()
        {
            return _documentType.List(x => x.DocumentTypeStatus == true);
        }
    }
}
