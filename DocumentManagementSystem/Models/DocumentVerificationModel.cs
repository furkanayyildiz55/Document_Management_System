using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentManagementSystem.Models
{
    public class DocumentVerificationModel
    {
        public string VerificationCode { get; set; }
        public Document document { get; set; }
        public bool isPostMethod { get; set; }
    }
}