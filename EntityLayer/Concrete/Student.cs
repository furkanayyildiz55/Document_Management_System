using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace EntityLayer.Concrete
{
    public class Student
    {
        [Key, Required]
        public int StudentNo { get; set; }

        [StringLength(50), Required]
        public string StudentName { get; set; }

        [StringLength(50), Required]
        public string StudentSurname { get; set; }

        [StringLength(50), Required]
        public string StudentProgram{ get; set; }

        [StringLength(50), Required]
        public string StudentMail { get; set; }

        public ICollection<Document> Documents { get; set; }


    }
}
