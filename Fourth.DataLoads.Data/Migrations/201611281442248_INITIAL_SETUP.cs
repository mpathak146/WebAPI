namespace Fourth.DataLoads.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class INITIAL_SETUP : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.t_DataLoadBatch",
                c => new
                    {
                        DataLoadBatchId = c.Long(nullable: false, identity: true),
                        DataloadTypeRefID = c.Long(nullable: false),
                        GroupID = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateProcessed = c.DateTime(),
                        Status = c.String(maxLength: 32),
                        UserName = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.DataLoadBatchId)
                .ForeignKey("dbo.t_DataLoadType", t => t.DataloadTypeRefID, cascadeDelete: false)
                .Index(t => t.DataloadTypeRefID);
            
            CreateTable(
                "dbo.t_DataLoadErrors",
                c => new
                    {
                        DataLoadErrorsId = c.Long(nullable: false, identity: true),
                        ErrRecord = c.String(storeType: "xml"),
                        ErrDescription = c.String(),
                        DataLoadBatchRefId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.DataLoadErrorsId)
                .ForeignKey("dbo.t_DataLoadBatch", t => t.DataLoadBatchRefId, cascadeDelete: false)
                .Index(t => t.DataLoadBatchRefId);
            
            CreateTable(
                "dbo.t_DataLoadType",
                c => new
                    {
                        DataloadTypeID = c.Long(nullable: false),
                        DataloadType = c.String(),
                    })
                .PrimaryKey(t => t.DataloadTypeID);
            
            CreateTable(
                "dbo.t_MassTermination",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DataLoadBatchRefId = c.Long(nullable: false),
                        EmployeeNumber = c.String(nullable: false, maxLength: 128),
                        TerminationDate = c.DateTime(nullable: false),
                        TerminationReason = c.String(nullable: false, maxLength: 512),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.t_DataLoadBatch", t => t.DataLoadBatchRefId, cascadeDelete: false)
                .Index(t => t.DataLoadBatchRefId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.t_MassTermination", "DataLoadBatchRefId", "dbo.t_DataLoadBatch");
            DropForeignKey("dbo.t_DataLoadBatch", "DataloadTypeRefID", "dbo.t_DataLoadType");
            DropForeignKey("dbo.t_DataLoadErrors", "DataLoadBatchRefId", "dbo.t_DataLoadBatch");
            DropIndex("dbo.t_MassTermination", new[] { "DataLoadBatchRefId" });
            DropIndex("dbo.t_DataLoadErrors", new[] { "DataLoadBatchRefId" });
            DropIndex("dbo.t_DataLoadBatch", new[] { "DataloadTypeRefID" });
            DropTable("dbo.t_MassTermination");
            DropTable("dbo.t_DataLoadType");
            DropTable("dbo.t_DataLoadErrors");
            DropTable("dbo.t_DataLoadBatch");
        }
    }
}
