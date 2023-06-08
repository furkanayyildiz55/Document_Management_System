using EntityLayer.Concrete;
using System.Data.Entity;


namespace DataAccessLayer.Concrete
{
    public class Context : DbContext
    {
        public DbSet<DocumentType>  DocumentTypes { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<DocumentTypeSignature> DocumentTypeSignatures { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<DocumentSignature> DocumentSignatures { get; set; }
        public DbSet<Setting> Settings { get; set; }
    }
}
