using System.Collections.Generic;
using EntityLayer.Concrete;

namespace BusinessLayer.Abstract
{
    public interface IDocumentSignatureService
    {
        void DocumentSignatureAdd(DocumentSignature documentSignature);
        void DocumentSignatureDelete(DocumentSignature documentSignature);
        void DocumentSignatureUpdate(DocumentSignature documentSignature);
        DocumentSignature GetDocumentSignature(int documentSignatureID);
        List<DocumentSignature> GetList(int DocumentID);
    }
}
