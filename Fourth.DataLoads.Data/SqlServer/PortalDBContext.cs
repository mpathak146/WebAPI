namespace Fourth.DataLoads.Data
{
    using Entities;
    using System.Data.Entity;

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

        // Add the entities we are using in here
        public virtual DbSet<DefaultHolidayAllowanceTable> DefaultHolidayAllowances { get; set; }

        public virtual DbSet<JobTitleTable> JobTitles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}