using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentManagementSystem.Models
{
    public class DocumentCreateModel
    {
        //TODO: jS işlemleri için kullanlacak model
        //TODO: Raporlama arayüzleri denenecek
        public string StudentFullName { get; set; }
        public string StudentNoMail { get; set; }
        public string StudentProgram { get; set; }

        public string DocumentName { get; set; }
        public string DocumentText { get; set; }
        public string DocumentVerificationCode { get; set; }
        public string DocumentCreateDate  { get; set; }
        public string DocumentBacgrounImage { get; set; }
        public List<DocumentCreateSignature> documentCreateSignatures = new List<DocumentCreateSignature>();

    }

    public class DocumentCreateSignature
    {
        public string AdminFullName { get; set; }
        public string AdminJob { get; set; }
        public string AdminSignatureImage { get; set; }
        public bool SignatureStatus { get; set; }
        public string SignatureAlign { get; set; }
    }

    
}