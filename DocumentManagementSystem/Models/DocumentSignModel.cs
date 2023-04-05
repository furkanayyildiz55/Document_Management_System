using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentManagementSystem.Models
{
    public class DocumentSignModel
    {
        List<DocumentSignature> documentSignatures { get; set; }
        int id { get; set; }
    }
}