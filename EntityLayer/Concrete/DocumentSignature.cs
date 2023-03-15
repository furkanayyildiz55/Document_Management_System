using System;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class DocumentSignature
    {
        [Key]
        public int DocumentSignatureID { get; set; }

        public bool DocumentSignatureStatus { get; set; }

        public int DocumentTypeSignatureID { get; set; }
        public virtual DocumentTypeSignature DocumentTypeSignature { get; set; }

        public int DocumentID { get; set; }
        public virtual Document Document { get; set; }
    }
}
