namespace Log4Pro.IS.TRM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MonitorDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WorkstationType = c.String(maxLength: 50),
                        Instance = c.String(maxLength: 50),
                        Timestamp = c.DateTime(nullable: false),
                        Content = c.String(storeType: "xml"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.WorkstationType)
                .Index(t => t.Instance);
            
            CreateTable(
                "dbo.Parts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PartNumber = c.String(nullable: false, maxLength: 50),
                        ExternalPartNumber = c.String(nullable: false, maxLength: 50),
                        SupplierId = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId)
                .Index(t => t.PartNumber, unique: true)
                .Index(t => t.ExternalPartNumber, unique: true)
                .Index(t => t.SupplierId);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Description, unique: true);
            
            CreateTable(
                "dbo.ShippingUnits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ShippingUnitId = c.String(maxLength: 50),
                        ExternalShippingUnitId = c.String(maxLength: 50),
                        ShippingUnitStatus = c.String(maxLength: 30),
                        PartId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        CreaterTransactionId = c.Int(nullable: false),
                        CloserTransactionId = c.Int(),
                        CloserTranzaction_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Transactions", t => t.CloserTranzaction_Id)
                .ForeignKey("dbo.Transactions", t => t.CreaterTransactionId)
                .ForeignKey("dbo.Parts", t => t.PartId)
                .Index(t => t.ShippingUnitId)
                .Index(t => t.ExternalShippingUnitId)
                .Index(t => t.ShippingUnitStatus)
                .Index(t => t.PartId)
                .Index(t => t.Active)
                .Index(t => t.CreaterTransactionId)
                .Index(t => t.CloserTranzaction_Id);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        TransactionType = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Timestamp)
                .Index(t => t.TransactionType);
            
            CreateTable(
                "dbo.PackagingUnits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PackageUnitId = c.String(maxLength: 128),
                        PackagingUnitStatus = c.String(maxLength: 30),
                        PartId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        CreaterTransactionId = c.Int(nullable: false),
                        CloserTransactionId = c.Int(),
                        CloserTranzaction_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Transactions", t => t.CloserTranzaction_Id)
                .ForeignKey("dbo.Transactions", t => t.CreaterTransactionId)
                .ForeignKey("dbo.Parts", t => t.PartId)
                .Index(t => t.PackageUnitId)
                .Index(t => t.PackagingUnitStatus)
                .Index(t => t.PartId)
                .Index(t => t.Active)
                .Index(t => t.CreaterTransactionId)
                .Index(t => t.CloserTranzaction_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PackagingUnits", "PartId", "dbo.Parts");
            DropForeignKey("dbo.PackagingUnits", "CreaterTransactionId", "dbo.Transactions");
            DropForeignKey("dbo.PackagingUnits", "CloserTranzaction_Id", "dbo.Transactions");
            DropForeignKey("dbo.ShippingUnits", "PartId", "dbo.Parts");
            DropForeignKey("dbo.ShippingUnits", "CreaterTransactionId", "dbo.Transactions");
            DropForeignKey("dbo.ShippingUnits", "CloserTranzaction_Id", "dbo.Transactions");
            DropForeignKey("dbo.Parts", "SupplierId", "dbo.Suppliers");
            DropIndex("dbo.PackagingUnits", new[] { "CloserTranzaction_Id" });
            DropIndex("dbo.PackagingUnits", new[] { "CreaterTransactionId" });
            DropIndex("dbo.PackagingUnits", new[] { "Active" });
            DropIndex("dbo.PackagingUnits", new[] { "PartId" });
            DropIndex("dbo.PackagingUnits", new[] { "PackagingUnitStatus" });
            DropIndex("dbo.PackagingUnits", new[] { "PackageUnitId" });
            DropIndex("dbo.Transactions", new[] { "TransactionType" });
            DropIndex("dbo.Transactions", new[] { "Timestamp" });
            DropIndex("dbo.ShippingUnits", new[] { "CloserTranzaction_Id" });
            DropIndex("dbo.ShippingUnits", new[] { "CreaterTransactionId" });
            DropIndex("dbo.ShippingUnits", new[] { "Active" });
            DropIndex("dbo.ShippingUnits", new[] { "PartId" });
            DropIndex("dbo.ShippingUnits", new[] { "ShippingUnitStatus" });
            DropIndex("dbo.ShippingUnits", new[] { "ExternalShippingUnitId" });
            DropIndex("dbo.ShippingUnits", new[] { "ShippingUnitId" });
            DropIndex("dbo.Suppliers", new[] { "Description" });
            DropIndex("dbo.Parts", new[] { "SupplierId" });
            DropIndex("dbo.Parts", new[] { "ExternalPartNumber" });
            DropIndex("dbo.Parts", new[] { "PartNumber" });
            DropIndex("dbo.MonitorDatas", new[] { "Instance" });
            DropIndex("dbo.MonitorDatas", new[] { "WorkstationType" });
            DropTable("dbo.PackagingUnits");
            DropTable("dbo.Transactions");
            DropTable("dbo.ShippingUnits");
            DropTable("dbo.Suppliers");
            DropTable("dbo.Parts");
            DropTable("dbo.MonitorDatas");
        }
    }
}
