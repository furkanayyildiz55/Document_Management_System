using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace EntityLayer.Concrete
{
    public class DocumentType
    {
        [Key,Required]
        public int DocumentTypeID { get; set; }

        [StringLength(50), Required]
        public string DocumentTypeName { get; set; }

        [StringLength(500), Required]
        public string DocumentTypeText { get; set; }

        [Required]
        public int DocumentTypeNumSignature { get; set; }

        [Required]
        public DateTime DocumentCreateDate { get; set; }

        [Required]
        public bool DocumentTypeStatus { get; set; }

        [StringLength(500), Required]
        public string DocumentTypeBacgroundImage { get; set; }

        public ICollection<DocumentSignature> DocumentSignatures { get; set; }

        public ICollection<Document> Documents { get; set; }




    }
}
