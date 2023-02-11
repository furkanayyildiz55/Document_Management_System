using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace EntityLayer.Concrete
{
    public class Student
    {
        [Key]
        public int StudentID { get; set; }

        public string StudentName { get; set; }

        public string StudentSurname { get; set; }

        public bool StudentUniversityRegistered { get; set; }

        public string StudentNo { get; set; }



        public string StudentProgram{ get; set; }

        public string StudentMail { get; set; }

        public ICollection<Document> Documents { get; set; }


    }
}
