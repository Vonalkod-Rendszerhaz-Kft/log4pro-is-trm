namespace Log4Pro.IS.TRM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.KanbanLocations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MonitorDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Instance = c.Int(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        Content = c.String(storeType: "xml"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Parts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductNumber = c.String(),
                        ExternalProductNumber = c.String(),
                        Description = c.String(),
                        Supplier_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Suppliers", t => t.Supplier_Id)
                .Index(t => t.Supplier_Id);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ShippingUnits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ShippingUnitId = c.String(),
                        ExternalShippingUnitId = c.String(),
                        Quantity = c.Int(nullable: false),
                        Create = c.DateTime(nullable: false),
                        Recieved = c.DateTime(),
                        Location_Id = c.Int(),
                        Part_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.Location_Id)
                .ForeignKey("dbo.Parts", t => t.Part_Id)
                .Index(t => t.Location_Id)
                .Index(t => t.Part_Id);
            
            CreateTable(
                "dbo.PackagingUnits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PackageUnitId = c.String(),
                        Quantity = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Part_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Parts", t => t.Part_Id)
                .Index(t => t.Part_Id);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.KanbanLoadings",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Location_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Transactions", t => t.Id)
                .ForeignKey("dbo.KanbanLocations", t => t.Location_Id)
                .Index(t => t.Id)
                .Index(t => t.Location_Id);
            
            CreateTable(
                "dbo.KanbanUnloadings",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        PackagingUnit_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Transactions", t => t.Id)
                .ForeignKey("dbo.PackagingUnits", t => t.PackagingUnit_Id)
                .Index(t => t.Id)
                .Index(t => t.PackagingUnit_Id);
            
            CreateTable(
                "dbo.Packagings",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        PackagingUnit_Id = c.Int(),
                        ShippingUnit_Id = c.Int(),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Transactions", t => t.Id)
                .ForeignKey("dbo.PackagingUnits", t => t.PackagingUnit_Id)
                .ForeignKey("dbo.ShippingUnits", t => t.ShippingUnit_Id)
                .Index(t => t.Id)
                .Index(t => t.PackagingUnit_Id)
                .Index(t => t.ShippingUnit_Id);
            
            CreateTable(
                "dbo.Putouts",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Transactions", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Receives",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Location_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Transactions", t => t.Id)
                .ForeignKey("dbo.Locations", t => t.Location_Id)
                .Index(t => t.Id)
                .Index(t => t.Location_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Receives", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.Receives", "Id", "dbo.Transactions");
            DropForeignKey("dbo.Putouts", "Id", "dbo.Transactions");
            DropForeignKey("dbo.Packagings", "ShippingUnit_Id", "dbo.ShippingUnits");
            DropForeignKey("dbo.Packagings", "PackagingUnit_Id", "dbo.PackagingUnits");
            DropForeignKey("dbo.Packagings", "Id", "dbo.Transactions");
            DropForeignKey("dbo.KanbanUnloadings", "PackagingUnit_Id", "dbo.PackagingUnits");
            DropForeignKey("dbo.KanbanUnloadings", "Id", "dbo.Transactions");
            DropForeignKey("dbo.KanbanLoadings", "Location_Id", "dbo.KanbanLocations");
            DropForeignKey("dbo.KanbanLoadings", "Id", "dbo.Transactions");
            DropForeignKey("dbo.PackagingUnits", "Part_Id", "dbo.Parts");
            DropForeignKey("dbo.ShippingUnits", "Part_Id", "dbo.Parts");
            DropForeignKey("dbo.ShippingUnits", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.Parts", "Supplier_Id", "dbo.Suppliers");
            DropIndex("dbo.Receives", new[] { "Location_Id" });
            DropIndex("dbo.Receives", new[] { "Id" });
            DropIndex("dbo.Putouts", new[] { "Id" });
            DropIndex("dbo.Packagings", new[] { "ShippingUnit_Id" });
            DropIndex("dbo.Packagings", new[] { "PackagingUnit_Id" });
            DropIndex("dbo.Packagings", new[] { "Id" });
            DropIndex("dbo.KanbanUnloadings", new[] { "PackagingUnit_Id" });
            DropIndex("dbo.KanbanUnloadings", new[] { "Id" });
            DropIndex("dbo.KanbanLoadings", new[] { "Location_Id" });
            DropIndex("dbo.KanbanLoadings", new[] { "Id" });
            DropIndex("dbo.PackagingUnits", new[] { "Part_Id" });
            DropIndex("dbo.ShippingUnits", new[] { "Part_Id" });
            DropIndex("dbo.ShippingUnits", new[] { "Location_Id" });
            DropIndex("dbo.Parts", new[] { "Supplier_Id" });
            DropTable("dbo.Receives");
            DropTable("dbo.Putouts");
            DropTable("dbo.Packagings");
            DropTable("dbo.KanbanUnloadings");
            DropTable("dbo.KanbanLoadings");
            DropTable("dbo.Transactions");
            DropTable("dbo.PackagingUnits");
            DropTable("dbo.ShippingUnits");
            DropTable("dbo.Suppliers");
            DropTable("dbo.Parts");
            DropTable("dbo.MonitorDatas");
            DropTable("dbo.Locations");
            DropTable("dbo.KanbanLocations");
        }
    }
}
