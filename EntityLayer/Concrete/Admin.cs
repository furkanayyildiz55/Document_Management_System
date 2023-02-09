using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class Admin
    {
        [Key, Required]
        public int AdminID { get; set; }

        [StringLength(50), Required]
        public string AdminName { get; set; }

        [StringLength(50), Required]
        public string AdminSurmane { get; set; }

        [StringLength(50), Required]
        public string AdminMail { get; set; }

        [StringLength(50), Required]
        public string AdminJob { get; set; }

        [StringLength(250)]
        public string AdminSignatureImage { get; set; }

        [Required]
        public bool AdminAuthorization { get; set; }

        [Required]
        public bool AdminStatus { get; set; }

        public ICollection<DocumentSignature> DocumentSignatures { get; set; }

        public ICollection<Document> Documents { get; set; }

    }
}
