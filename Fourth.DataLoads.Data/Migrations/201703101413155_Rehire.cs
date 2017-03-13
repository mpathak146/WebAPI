namespace Fourth.DataLoads.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rehire : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.t_MassRehire",
                c => new
                    {
                        MassRehireId = c.Long(nullable: false, identity: true),
                        DataLoadJobRefId = c.Guid(nullable: false),
                        DataLoadBatchRefId = c.Guid(nullable: false),
                        EmployeeNumber = c.String(nullable: false, maxLength: 128),
                        RehireDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MassRehireId)
                .ForeignKey("dbo.t_DataLoad", t => new { t.DataLoadJobRefId, t.DataLoadBatchRefId }, cascadeDelete: false)
                .Index(t => new { t.DataLoadJobRefId, t.DataLoadBatchRefId });
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.t_MassRehire", new[] { "DataLoadJobRefId", "DataLoadBatchRefId" }, "dbo.t_DataLoad");
            DropIndex("dbo.t_MassRehire", new[] { "DataLoadJobRefId", "DataLoadBatchRefId" });
            DropTable("dbo.t_MassRehire");
        }
    }
}
