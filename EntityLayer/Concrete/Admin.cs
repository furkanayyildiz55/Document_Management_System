using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class Admin
    {
        [Key]
        public int AdminID { get; set; }

        public string AdminName { get; set; }

        public string AdminSurmane { get; set; }

        public string AdminMail { get; set; }

        public string AdminJob { get; set; }

        public string AdminSignatureImage { get; set; }

        public bool AdminAuthorization { get; set; }

        public bool AdminStatus { get; set; }

        public ICollection<DocumentSignature> DocumentSignatures { get; set; }

        public ICollection<Document> Documents { get; set; }

    }
}
