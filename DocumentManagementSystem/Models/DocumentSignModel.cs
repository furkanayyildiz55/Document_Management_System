using System;
using System.Collections.Generic;
using static DataAccessLayer.Concrete.Repositories.Repository;

namespace DocumentManagementSystem.Models
{
    public class DocumentSignModel
    {
        //SignedDocument Classı Repositoryden alındı
        public List<SignedDocument> SignedDocumentList { get; set; }
        public string DocumentSignatureID { get; set; }
        public string DocumentID { get; set; }
    }


}