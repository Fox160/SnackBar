namespace SnackBarService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                        OutputID = c.Int(nullable: false),
                        ExecutorID = c.Int(),
                        Count = c.Int(nullable: false),
                        Summa = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        DateOfCreate = c.DateTime(nullable: false),
                        DateOfImplement = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customers", t => t.CustomerID, cascadeDelete: true)
                .ForeignKey("dbo.Executors", t => t.ExecutorID)
                .ForeignKey("dbo.Outputs", t => t.OutputID, cascadeDelete: true)
                .Index(t => t.CustomerID)
                .Index(t => t.OutputID)
                .Index(t => t.ExecutorID);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CustomerFullName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Executors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ExecutorFullName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Outputs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OutputName = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OutputElements",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OutputID = c.Int(nullable: false),
                        ElementID = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Elements", t => t.ElementID, cascadeDelete: true)
                .ForeignKey("dbo.Outputs", t => t.OutputID, cascadeDelete: true)
                .Index(t => t.OutputID)
                .Index(t => t.ElementID);
            
            CreateTable(
                "dbo.Elements",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ElementName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ReserveElements",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ReserveID = c.Int(nullable: false),
                        ElementID = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Elements", t => t.ElementID, cascadeDelete: true)
                .ForeignKey("dbo.Reserves", t => t.ReserveID, cascadeDelete: true)
                .Index(t => t.ReserveID)
                .Index(t => t.ElementID);
            
            CreateTable(
                "dbo.Reserves",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ReserveName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OutputElements", "OutputID", "dbo.Outputs");
            DropForeignKey("dbo.ReserveElements", "ReserveID", "dbo.Reserves");
            DropForeignKey("dbo.ReserveElements", "ElementID", "dbo.Elements");
            DropForeignKey("dbo.OutputElements", "ElementID", "dbo.Elements");
            DropForeignKey("dbo.Bookings", "OutputID", "dbo.Outputs");
            DropForeignKey("dbo.Bookings", "ExecutorID", "dbo.Executors");
            DropForeignKey("dbo.Bookings", "CustomerID", "dbo.Customers");
            DropIndex("dbo.ReserveElements", new[] { "ElementID" });
            DropIndex("dbo.ReserveElements", new[] { "ReserveID" });
            DropIndex("dbo.OutputElements", new[] { "ElementID" });
            DropIndex("dbo.OutputElements", new[] { "OutputID" });
            DropIndex("dbo.Bookings", new[] { "ExecutorID" });
            DropIndex("dbo.Bookings", new[] { "OutputID" });
            DropIndex("dbo.Bookings", new[] { "CustomerID" });
            DropTable("dbo.Reserves");
            DropTable("dbo.ReserveElements");
            DropTable("dbo.Elements");
            DropTable("dbo.OutputElements");
            DropTable("dbo.Outputs");
            DropTable("dbo.Executors");
            DropTable("dbo.Customers");
            DropTable("dbo.Bookings");
        }
    }
}
