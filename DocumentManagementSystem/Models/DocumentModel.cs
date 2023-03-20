using EntityLayer.Concrete;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DocumentManagementSystem.Models
{
    public class DocumentModel
    {
        public List<SelectListItem> selectDocumentTypeItems { get; set; }
        public Document document { get; set; }
        public string studentNo { get; set; }
    }
}