namespace Log4Pro.IS.TRM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Part_Entity_Add_SupplierShippingUnitQty_Field : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Parts", "SupplierShippingUnitQty", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Parts", "SupplierShippingUnitQty");
        }
    }
}
