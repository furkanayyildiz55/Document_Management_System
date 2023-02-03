using System;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class DocumentSignature
    {
        [Key]
        public int DocumentSignatureID { get; set; }

        [StringLength(50), Required]
        public string DocumentSignatureAlign { get; set; }

        [Required]
        public bool DocumentSignatureStatus { get; set; }

        
        public int AdminID { get; set; }
        public virtual Admin Admin { get; set; }

        
        public int DocumentTypeID { get; set; }
        public virtual DocumentType DocumentType { get; set; }


    }
}
