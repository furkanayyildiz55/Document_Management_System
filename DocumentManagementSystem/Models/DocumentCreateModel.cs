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
        string StudentFullName { get; set; }
        string StudentNoMail { get; set; }
        string StudentProgram { get; set; }

        string DocumentName { get; set; }
        string DocumentText { get; set; }
        string DocumentVerificationCode { get; set; }
        string DocumentCreateDate  { get; set; }
        string DocumentBacgrounImage { get; set; }
        List<DocumentCreateSignature> documentCreateSignatures { get; set; }

    }

    public class DocumentCreateSignature
    {
        string AdminFullName { get; set; }
        string AdminJob { get; set; }
        string AdminSignatureImage { get; set; }
        bool SignatureStatus { get; set; }
        string SignatureAlign { get; set; }
    }

    
}