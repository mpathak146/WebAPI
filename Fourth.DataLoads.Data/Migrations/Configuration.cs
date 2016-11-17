namespace Fourth.DataLoads.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Fourth.DataLoads.Data.DataloadsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Fourth.DataLoads.Data.DataloadsContext context)
        {
            //context.MassTerminations.AddOrUpdate(new Entities.MassTermination { BatchID = new System.Guid().ToString(), EmployeeNumber = "1", TerminationDate = DateTime.Parse("1 dec 2014"), TerminationReason = "Just like that" });
        }
        
    }
}
