using System;
using System.Collections.Generic;
using static DataAccessLayer.Concrete.Repositories.Repository;

namespace DocumentManagementSystem.Models
{
    public class DocumentSignModel
    {
        //Class Repositoryden alındı
        public List<SignedDocument> SignedDocumentList { get; set; }
        public string DocumentSignatureID { get; set; }
    }


}