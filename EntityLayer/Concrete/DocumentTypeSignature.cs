using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityLayer.Concrete
{
    public class DocumentTypeSignature
    {
        [Key]
        public int DocumentTypeSignatureID { get; set; }

        public string DocumentTypeSignatureAlign { get; set; }

        [ForeignKey("AdminID")]
        public virtual Admin Admin { get; set; }
        public int? AdminID { get; set; }
        
        [ForeignKey("DocumentTypeID")]
        public virtual DocumentType DocumentType { get; set; }
        public int? DocumentTypeID { get; set; }

        public ICollection<DocumentSignature> DocumentSignatures { get; set; }

    }
}
