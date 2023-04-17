namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig_password_Add : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Admins", "AdminPassword", c => c.String());
            AddColumn("dbo.Students", "StudentPassword", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "StudentPassword");
            DropColumn("dbo.Admins", "AdminPassword");
        }
    }
}
