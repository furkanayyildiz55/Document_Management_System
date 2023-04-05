using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System.Collections.Generic;


namespace BusinessLayer.Concrete
{
    public class DocumentTypeSignatureManager : IDocumentTypeSignatureService
    {
        IDocumentTypeSignatureDal _documentTypeSignature;

        public DocumentTypeSignatureManager(IDocumentTypeSignatureDal documentTypeSignature)
        {
            _documentTypeSignature = documentTypeSignature;
        }


        public void DocumentTypeSignatureAdd(DocumentTypeSignature documentTypeSignature)
        {
            _documentTypeSignature.Add(documentTypeSignature);
        }

        public void DocumentTypeSignatureDelete(DocumentTypeSignature documentTypeSignature)
        {
            _documentTypeSignature.Delete(documentTypeSignature);
        }

        public void DocumentTypeSignatureUpdate(DocumentTypeSignature documentTypeSignature)
        {
            _documentTypeSignature.Update(documentTypeSignature);
        }

        public DocumentTypeSignature GetDocumentTypeSignature(int documentTypeSignatureID)
        {
            return _documentTypeSignature.Get(x => x.DocumentTypeSignatureID == documentTypeSignatureID);
        }

        public List<DocumentTypeSignature> GetListToDocumentTypeSignature(int documentTypeID  )
        {
            return _documentTypeSignature.List(x => x.DocumentTypeID == documentTypeID);
        }

        ///
        ///

        public List<DocumentTypeSignature> GetListToDocumentTypeSignatureWithAdminID(int AdminID)
        {
            return _documentTypeSignature.List(x => x.AdminID == AdminID  ) ;
        }
        
    }
}
