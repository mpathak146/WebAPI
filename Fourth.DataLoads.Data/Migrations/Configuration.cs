namespace Fourth.DataLoads.Data.Migrations
{
    using Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Fourth.DataLoads.Data.StagingDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Fourth.DataLoads.Data.StagingDBContext context)
        {
            context.DataLoadType.AddOrUpdate(
                Enum.GetValues(typeof(DataLoadTypes))
                    .OfType<DataLoadTypes>()
                    .Select(
                    x => new DataLoadType
                    {
                        DataloadTypeID = (long)x,
                        DataloadType = x.ToString(),
                    })
                        .ToArray());
        }
        
    }
}
