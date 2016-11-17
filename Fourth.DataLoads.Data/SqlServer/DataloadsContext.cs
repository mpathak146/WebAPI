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
            : base(connectionString!=null?connectionString:"DataloadsContext")
        {   
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataloadsContext, 
                Migrations.Configuration>());
        }
        public DataloadsContext()
            : base("DataloadsContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataloadsContext, 
                Migrations.Configuration>());
        }
        public virtual DbSet<MassTermination> MassTerminations {get; set;}
        public virtual DbSet<DataLoadBatch> DataLoadBatch { get; set; }
        public virtual DbSet<DataLoadType> DataLoadType { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MassTermination>().Property(m => m.EmployeeNumber).HasMaxLength(128).IsRequired();
            modelBuilder.Entity<MassTermination>().Property(m => m.TerminationReason).HasMaxLength(512).IsRequired();
            modelBuilder.Entity<DataLoadBatch>().Property(d => d.Status).HasMaxLength(32);
            modelBuilder.Entity<DataLoadBatch>().Property(d => d.UserName).HasMaxLength(128);

        }
    }
}
