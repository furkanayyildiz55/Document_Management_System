using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentManagementSystem.Models
{
    public class SignInModel
    {
        public string UserMail { get; set; }
        public string UserPassword { get; set; }
        public bool UserIsAdmin { get; set; }
    }
}