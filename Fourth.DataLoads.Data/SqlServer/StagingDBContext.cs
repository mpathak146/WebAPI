namespace Fourth.DataLoads.Data
{
    using System;
    using System.Data.Entity;
    using Fourth.DataLoads.Data.Entities;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class StagingDBContext : DbContext
    {
        public string connectionString;
        public StagingDBContext(string connectionString)
            : base(connectionString!=null?connectionString:"DataloadsContext")
        {   
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<StagingDBContext, 
                Migrations.Configuration>());
            this.connectionString = connectionString;
        }
        public StagingDBContext()
            : base("DataloadsContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<StagingDBContext, 
                Migrations.Configuration>());
        }
        public virtual DbSet<MassTermination> MassTerminations {get; set;}
        public virtual DbSet<DataLoad> DataLoad { get; set; }
        public virtual DbSet<DataLoadType> DataLoadType { get; set; }
        public virtual DbSet<DataLoadErrors> DataLoadErrors { get; set; }


        //public virtual DbSet<TableSchema> TableSchema { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MassTermination>().Property(m => m.EmployeeNumber).HasMaxLength(128).IsRequired();
            modelBuilder.Entity<MassTermination>().Property(m => m.TerminationReason).HasMaxLength(512).IsRequired();

            modelBuilder.Entity<DataLoad>().Property(d => d.Status).HasMaxLength(32);
            modelBuilder.Entity<DataLoad>().Property(d => d.UserName).HasMaxLength(128);
            modelBuilder.Entity<DataLoad>().Property(d => d.DateProcessed).IsOptional();
            

            modelBuilder.Entity<DataLoadErrors>().Property(e => e.DataLoadBatchRefId).IsRequired();

            modelBuilder.Entity<DataLoadType>().Property(t => t.DataloadTypeID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)
                .IsRequired();
            

            //modelBuilder.Entity<DataLoadBatch>().HasOptional(t => t.MassesToTerminate)
            //    .WithOptionalDependent()
            //    .WillCascadeOnDelete(false);
            //modelBuilder.Entity<DataLoadBatch>().HasOptional(t => t.dataloadErrors)
            //    .WithOptionalDependent()
            //    .WillCascadeOnDelete(false);
            //modelBuilder.Entity<DataLoadType>().HasOptional(x=>x.Batches)
            //    .WithOptionalDependent()
            //    .WillCascadeOnDelete(false);

        }
    }
}
