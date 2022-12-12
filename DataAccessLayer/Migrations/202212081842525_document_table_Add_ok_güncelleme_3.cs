namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class document_table_Add_ok_güncelleme_3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "DocumentStudentNo", c => c.Int(nullable: false));
            DropColumn("dbo.Documents", "DocumentStudentID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Documents", "DocumentStudentID", c => c.Int(nullable: false));
            DropColumn("dbo.Documents", "DocumentStudentNo");
        }
    }
}
