using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace EntityModel.DomainModel
{
    public class DiContext : DbContext
    {
        // Your context has been configured to use a 'DiContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'EntityModel.DomainModel.DiContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'DiContext' 
        // connection string in the application configuration file.
        
        //public DiContext()
        //    : base("name=DiContext")
        //{
        //}

        public DiContext(string conString) : base(conString)
        {
            Database.SetInitializer<DiContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.
        //public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<RegisterCustomer> Register { get; set; }
        public virtual DbSet<Property> Property { get; set; }
        public virtual DbSet<Setting> Setting { get; set; }
    }
}