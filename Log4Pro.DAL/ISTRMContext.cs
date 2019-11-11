namespace Log4Pro.IS.TRM.DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;
    using VRH.ConnectionStringStore;

    public class ISTRMContext : DbContext
    {
        // Your context has been configured to use a 'Log4ProContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Log4Pro.DAL.Log4ProContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'Log4ProContext' 
        // connection string in the application configuration file.
        public ISTRMContext()
            : base(VRHConnectionStringStore.GetSQLConnectionString("IS-TRM", true))
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public virtual DbSet<PackagingUnit> Transaction { get; set; }
        public virtual DbSet<ShippingUnit> ShippingUnits { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<Part> Parts { get; set; }
        public virtual DbSet<MonitorData> MonitorDatas { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
    }

}