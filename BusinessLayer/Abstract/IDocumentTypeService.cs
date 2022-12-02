using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IDocumentTypeService
    {
        List<DocumentType> GetList();
        void DocumentTypeAdd(DocumentType documentType);
        void DocumentTypeDelete(DocumentType documentType);
        void DocumentTypeUpdate(DocumentType documentType);
        DocumentType GetDocumentType(int DocumentTypeID);
    }
}
