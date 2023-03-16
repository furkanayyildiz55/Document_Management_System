using System.Collections.Generic;
using EntityLayer.Concrete;


namespace BusinessLayer.Abstract
{
    public interface IDocumentTypeSignatureService
    {
        void DocumentTypeSignatureAdd(DocumentTypeSignature documentTypeSignature);
        void DocumentTypeSignatureDelete(DocumentTypeSignature documentTypeSignature);
        void DocumentTypeSignatureUpdate(DocumentTypeSignature documentTypeSignature);
        DocumentTypeSignature GetDocumentTypeSignature(int documentTypeSignature);
        List<DocumentTypeSignature> GetListToDocumentType(int documentTypeID);

    }
}
