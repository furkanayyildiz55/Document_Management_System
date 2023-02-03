using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class Document
    {
        [Key]
        public int DocumentID { get; set; }

        [StringLength(250),Required]
        public string DocumentPdfUrl { get; set; }

        [StringLength(50), Required]
        public string DocumentVerificationCode { get; set; }

        [Required]
        public DateTime  DocumentCreateDate { get; set; }

        [Required]
        public bool DocumentStatus { get; set; }

        public int DocumentTypeID { get; set; }
        public virtual DocumentType DocumentType { get; set; }

        public int StudentNo { get; set; }
        public virtual Student Student { get; set; }

        public int AdminID { get; set; }
        public virtual Admin Admin { get; set; }







    }
}
