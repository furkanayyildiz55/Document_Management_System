using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentManagementSystem.Models
{
    public class DocumentTypeModel
    {
        public List <SelectListItem> selectAdminItems { get; set; }
        public int[] AdminIds { get; set; }
        public DocumentType documentType { get; set; }

    }
}