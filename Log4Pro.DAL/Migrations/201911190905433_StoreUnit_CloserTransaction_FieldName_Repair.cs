namespace Log4Pro.IS.TRM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoreUnit_CloserTransaction_FieldName_Repair : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ShippingUnits", "CloserTransactionId");
            DropColumn("dbo.PackagingUnits", "CloserTransactionId");
            RenameColumn(table: "dbo.ShippingUnits", name: "CloserTranzaction_Id", newName: "CloserTransactionId");
            RenameColumn(table: "dbo.PackagingUnits", name: "CloserTranzaction_Id", newName: "CloserTransactionId");
            RenameIndex(table: "dbo.ShippingUnits", name: "IX_CloserTranzaction_Id", newName: "IX_CloserTransactionId");
            RenameIndex(table: "dbo.PackagingUnits", name: "IX_CloserTranzaction_Id", newName: "IX_CloserTransactionId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.PackagingUnits", name: "IX_CloserTransactionId", newName: "IX_CloserTranzaction_Id");
            RenameIndex(table: "dbo.ShippingUnits", name: "IX_CloserTransactionId", newName: "IX_CloserTranzaction_Id");
            RenameColumn(table: "dbo.PackagingUnits", name: "CloserTransactionId", newName: "CloserTranzaction_Id");
            RenameColumn(table: "dbo.ShippingUnits", name: "CloserTransactionId", newName: "CloserTranzaction_Id");
            AddColumn("dbo.PackagingUnits", "CloserTransactionId", c => c.Int());
            AddColumn("dbo.ShippingUnits", "CloserTransactionId", c => c.Int());
        }
    }
}
