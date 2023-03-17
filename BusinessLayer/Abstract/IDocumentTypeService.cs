using EntityLayer.Concrete;
using System.Collections.Generic;


namespace BusinessLayer.Abstract
{
    public interface IDocumentTypeService
    {
        List<DocumentType> GetList();
        List<DocumentType> GetListActiveDocument();
        int DocumentTypeAdd(DocumentType documentType);
        void DocumentTypeDelete(DocumentType documentType);
        void DocumentTypeUpdate(DocumentType documentType);
        DocumentType GetDocumentType(int DocumentTypeID);
    }
}
