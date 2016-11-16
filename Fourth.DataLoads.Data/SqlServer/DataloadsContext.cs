namespace Fourth.DataLoads.Data
{
    using System;
    using System.Data.Entity;
    using Fourth.DataLoads.Data.Entities;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DataloadsContext : DbContext
    {
        public DataloadsContext(string connectionString)
            : base(connectionString)
        {
            // Stop EF from creating or updating the underlying database
            Database.SetInitializer<DbContext>(null);
        }

        public virtual DbSet<MassTermination> MassTerminations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataloadsContext, Configuration>();
        }
    }
}
