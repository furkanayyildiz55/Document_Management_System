namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class document_table_Add : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        DocumentID = c.Int(nullable: false, identity: true),
                        DocumentPdfUrl = c.String(nullable: false, maxLength: 250),
                        DocumentVerificationCode = c.String(nullable: false, maxLength: 50),
                        DocumentCreateDate = c.DateTime(nullable: false),
                        DocumentStatus = c.Boolean(nullable: false),
                        DocumentTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DocumentID)
                .ForeignKey("dbo.DocumentTypes", t => t.DocumentTypeID, cascadeDelete: true)
                .Index(t => t.DocumentTypeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "DocumentTypeID", "dbo.DocumentTypes");
            DropIndex("dbo.Documents", new[] { "DocumentTypeID" });
            DropTable("dbo.Documents");
        }
    }
}
