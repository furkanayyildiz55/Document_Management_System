using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class DocumentTypeManager : IDocumentTypeService
    {
        IDocumentTypeDal _documentType;


        public DocumentTypeManager(IDocumentTypeDal documentType)
        {
            _documentType = documentType;
        }

        public void DocumentTypeAdd(DocumentType documentType)
        {
            _documentType.Add(documentType);
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
            throw new NotImplementedException();
        }

    }
}
