namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class document_table_Add_ok_güncelleme_ : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Documents", "Admin_AdminID", "dbo.Admins");
            DropForeignKey("dbo.Documents", "Student_StudentNo", "dbo.Students");
            DropIndex("dbo.Documents", new[] { "Admin_AdminID" });
            DropIndex("dbo.Documents", new[] { "Student_StudentNo" });
            RenameColumn(table: "dbo.Documents", name: "Admin_AdminID", newName: "AdminID");
            RenameColumn(table: "dbo.Documents", name: "Student_StudentNo", newName: "StudentNo");
            AlterColumn("dbo.Documents", "AdminID", c => c.Int(nullable: false));
            AlterColumn("dbo.Documents", "StudentNo", c => c.Int(nullable: false));
            CreateIndex("dbo.Documents", "StudentNo");
            CreateIndex("dbo.Documents", "AdminID");
            AddForeignKey("dbo.Documents", "AdminID", "dbo.Admins", "AdminID", cascadeDelete: true);
            AddForeignKey("dbo.Documents", "StudentNo", "dbo.Students", "StudentNo", cascadeDelete: true);
            DropColumn("dbo.Documents", "DocumentStudentNo");
            DropColumn("dbo.Documents", "DocumentCreatorAdminID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Documents", "DocumentCreatorAdminID", c => c.Int(nullable: false));
            AddColumn("dbo.Documents", "DocumentStudentNo", c => c.Int(nullable: false));
            DropForeignKey("dbo.Documents", "StudentNo", "dbo.Students");
            DropForeignKey("dbo.Documents", "AdminID", "dbo.Admins");
            DropIndex("dbo.Documents", new[] { "AdminID" });
            DropIndex("dbo.Documents", new[] { "StudentNo" });
            AlterColumn("dbo.Documents", "StudentNo", c => c.Int());
            AlterColumn("dbo.Documents", "AdminID", c => c.Int());
            RenameColumn(table: "dbo.Documents", name: "StudentNo", newName: "Student_StudentNo");
            RenameColumn(table: "dbo.Documents", name: "AdminID", newName: "Admin_AdminID");
            CreateIndex("dbo.Documents", "Student_StudentNo");
            CreateIndex("dbo.Documents", "Admin_AdminID");
            AddForeignKey("dbo.Documents", "Student_StudentNo", "dbo.Students", "StudentNo");
            AddForeignKey("dbo.Documents", "Admin_AdminID", "dbo.Admins", "AdminID");
        }
    }
}
