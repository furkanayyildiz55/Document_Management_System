using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class Document
    {
        [Key]
        public int DocumentID { get; set; }

        public string DocumentPdfUrl { get; set; }

        public string DocumentVerificationCode { get; set; }

        public DateTime  DocumentCreateDate { get; set; }

        public bool DocumentStatus { get; set; }

        public int DocumentTypeID { get; set; }
        public virtual DocumentType DocumentType { get; set; }

        public int StudentNo { get; set; }
        public virtual Student Student { get; set; }

        public int AdminID { get; set; }
        public virtual Admin Admin { get; set; }

        public ICollection<DocumentSignature> DocumentSignatures { get; set; }








    }
}
