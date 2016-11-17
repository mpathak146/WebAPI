namespace Fourth.DataLoads.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialmodel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.t_DataLoadBatch",
                c => new
                    {
                        DataLoadBatchId = c.Long(nullable: false, identity: true),
                        DataloadTypeID = c.Long(nullable: false),
                        GroupID = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateProcessed = c.DateTime(nullable: false),
                        Status = c.String(maxLength: 32),
                        UserName = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.DataLoadBatchId)
                .ForeignKey("dbo.t_DataLoadType", t => t.DataloadTypeID, cascadeDelete: true)
                .Index(t => t.DataloadTypeID);
            
            CreateTable(
                "dbo.t_DataLoadType",
                c => new
                    {
                        DataloadTypeID = c.Long(nullable: false, identity: true),
                        DataloadType = c.String(),
                    })
                .PrimaryKey(t => t.DataloadTypeID);
            
            CreateTable(
                "dbo.t_MassTermination",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DataLoadBatchId = c.Long(nullable: false),
                        EmployeeNumber = c.String(nullable: false, maxLength: 128),
                        TerminationDate = c.DateTime(nullable: false),
                        TerminationReason = c.String(nullable: false, maxLength: 512),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.t_DataLoadBatch", t => t.DataLoadBatchId, cascadeDelete: true)
                .Index(t => t.DataLoadBatchId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.t_MassTermination", "DataLoadBatchId", "dbo.t_DataLoadBatch");
            DropForeignKey("dbo.t_DataLoadBatch", "DataloadTypeID", "dbo.t_DataLoadType");
            DropIndex("dbo.t_MassTermination", new[] { "DataLoadBatchId" });
            DropIndex("dbo.t_DataLoadBatch", new[] { "DataloadTypeID" });
            DropTable("dbo.t_MassTermination");
            DropTable("dbo.t_DataLoadType");
            DropTable("dbo.t_DataLoadBatch");
        }
    }
}