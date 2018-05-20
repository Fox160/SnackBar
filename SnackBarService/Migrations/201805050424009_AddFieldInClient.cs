namespace SnackBarService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldInClient : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Mail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "Mail");
        }
    }
}
