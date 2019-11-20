namespace Log4Pro.IS.TRM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoreUnit_Add_StoreLocation_Field : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShippingUnits", "StoreLocation", c => c.String(maxLength: 30));
            CreateIndex("dbo.ShippingUnits", "StoreLocation");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ShippingUnits", new[] { "StoreLocation" });
            DropColumn("dbo.ShippingUnits", "StoreLocation");
        }
    }
}
