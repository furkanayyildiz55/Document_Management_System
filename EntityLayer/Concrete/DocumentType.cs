using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace EntityLayer.Concrete
{
    public class DocumentType
    {
        [Key]
        public int DocumentTypeID { get; set; }

        public string DocumentTypeName { get; set; }

        public string DocumentTypeText { get; set; }

        public int DocumentTypeNumSignature { get; set; }

        public DateTime DocumentTypeCreateDate { get; set; }

        public bool DocumentTypeStatus { get; set; }

        public string DocumentTypeBacgroundImage { get; set; }

        public ICollection<Document> Documents { get; set; }

        public ICollection<DocumentTypeSignature> DocumentTypeSignatures { get; set; }





    }
}
