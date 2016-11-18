namespace Fourth.DataLoads.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dateprocessednullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.t_DataLoadBatch", "DateProcessed", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.t_DataLoadBatch", "DateProcessed", c => c.DateTime(nullable: false));
        }
    }
}
