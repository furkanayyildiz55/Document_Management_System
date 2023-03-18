using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IDocumentService
    {

        List<Document> GetList();
        int DocumentAdd(Document document);
        void DocumentDelete(Document document);
        void DocumentUpdate(Document document);
        Document GetDocument(int document);
    }
}
