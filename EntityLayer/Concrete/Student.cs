using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace EntityLayer.Concrete
{
    // TODO : Belgeyi alacak olan kişi ünv. öğrencisi değil ise nasıl giriş yapacak bunu özellikle tutmak lazımmı, özellikle o kişilere şifre tutmak gerekir ! ?

    //TODO: Mezun olan öğrencilerin merkezi kimlik bilileri tutuluyormu, tutulmuyorsa veritabanına şifre istenmeli mi ?


    public class Student
    {
        [Key]
        public int StudentID { get; set; }

        public string StudentName { get; set; }

        public string StudentSurname { get; set; }

        public string StudentMail { get; set; }

        public bool StudentUniversityRegistered { get; set; }

        public string StudentNo { get; set; }

        public string StudentProgram{ get; set; }

        public ICollection<Document> Documents { get; set; }


    }
}
