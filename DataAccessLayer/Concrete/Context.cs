using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<DocumentSignature>()
        //        .HasRequired(c => c.DocumentTypeID)
        //        .WithMany()
        //        .WillCascadeOnDelete(false);
        //}



    }
}
