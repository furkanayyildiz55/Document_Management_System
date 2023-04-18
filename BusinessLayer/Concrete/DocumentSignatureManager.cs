using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System.Collections.Generic;

namespace BusinessLayer.Concrete
{
    public class DocumentSignatureManager : IDocumentSignatureService
    {

        IDocumentSignatureDal _documentSignature;

        public DocumentSignatureManager(IDocumentSignatureDal documentSignature)
        {
            _documentSignature = documentSignature;
        }


        public void DocumentSignatureAdd(DocumentSignature documentSignature)
        {
            _documentSignature.Add(documentSignature);
        }

        public void DocumentSignatureDelete(DocumentSignature documentSignature)
        {
            _documentSignature.Delete(documentSignature);
        }

        public void DocumentSignatureUpdate(DocumentSignature documentSignature)
        {
            _documentSignature.Update(documentSignature);
        }

        public DocumentSignature GetDocumentSignature(int documentSignatureID)
        {
            return _documentSignature.Get(x => x.DocumentSignatureID == documentSignatureID);
        }

        public List<DocumentSignature> GetList(int DocumentID)
        {
            return _documentSignature.List(x => x.DocumentID == DocumentID);
        }


        /////
        ///
        public List<DocumentSignature> GetListWithDocumentTypeSignatureID(int DocumentTypeSignatureID)
        {
            return _documentSignature.List(x => x.DocumentTypeSignatureID == DocumentTypeSignatureID);
        }

        public bool GetDocumentSignatureStatus(int DocumentID)
        {
           DocumentSignature Data = _documentSignature.GetFirstOrDefault(x => x.DocumentID == DocumentID && x.DocumentSignatureStatus == false);
            return Data == null ? true : false;
        }

    }
}
