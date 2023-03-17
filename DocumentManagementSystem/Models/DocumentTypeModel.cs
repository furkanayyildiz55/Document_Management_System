using EntityLayer.Concrete;
using System.Collections.Generic;
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