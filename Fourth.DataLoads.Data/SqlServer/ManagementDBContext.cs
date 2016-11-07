namespace Fourth.DataLoads.Data
{
    using Entities.ManagementDB;
    using System.Data.Entity;

    /// <summary>
    /// Manages connections to the People System management database.
    /// </summary>
    /// <remarks>
    /// This context is used to retrieve the group details for the chosen customer.
    /// </remarks>
    public partial class PSLiveManagementDBContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDbContext"/> class.
        /// </summary>
        public PSLiveManagementDBContext(string connectionString)
            : base(connectionString)
        {
            // Stop EF from creating or updating the underlying database
            Database.SetInitializer<DbContext>(null);
        }

        // Add entities here
        public virtual DbSet<DSNTable> Groups { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}