using System.Data.Entity;
using Fourth.DataLoads.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fourth.DataLoads.Data
{

    /// <summary>
    /// Manages connections to the People System database.
    /// </summary>
    public partial class PortalDBContext : DbContext
    {
        // Parameterless constructor used in mocking.
        internal PortalDBContext() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDbContext"/> class.
        /// </summary>
        public PortalDBContext(string connectionString) : base(connectionString)
        {
            // Stop EF from creating or updating the underlying database
            Database.SetInitializer<DbContext>(null);
        }
        public virtual DbSet<MassTerminationPortal> MassTerminationsPortal { get; set; }

        // Add the entities we are using in here
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}