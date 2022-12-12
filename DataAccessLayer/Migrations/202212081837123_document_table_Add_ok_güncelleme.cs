namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class document_table_Add_ok_güncelleme : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "DocumentStudentID", c => c.Int(nullable: false));
            AddColumn("dbo.Documents", "DocumentCreatorAdminID", c => c.Int(nullable: false));
            AddColumn("dbo.Documents", "Admin_AdminID", c => c.Int());
            AddColumn("dbo.Documents", "Student_StudentNo", c => c.Int());
            CreateIndex("dbo.Documents", "Admin_AdminID");
            CreateIndex("dbo.Documents", "Student_StudentNo");
            AddForeignKey("dbo.Documents", "Admin_AdminID", "dbo.Admins", "AdminID");
            AddForeignKey("dbo.Documents", "Student_StudentNo", "dbo.Students", "StudentNo");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "Student_StudentNo", "dbo.Students");
            DropForeignKey("dbo.Documents", "Admin_AdminID", "dbo.Admins");
            DropIndex("dbo.Documents", new[] { "Student_StudentNo" });
            DropIndex("dbo.Documents", new[] { "Admin_AdminID" });
            DropColumn("dbo.Documents", "Student_StudentNo");
            DropColumn("dbo.Documents", "Admin_AdminID");
            DropColumn("dbo.Documents", "DocumentCreatorAdminID");
            DropColumn("dbo.Documents", "DocumentStudentID");
        }
    }
}
