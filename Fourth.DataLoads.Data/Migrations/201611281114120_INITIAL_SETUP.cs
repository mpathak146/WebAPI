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
                        DataloadTypeID = c.Long(nullable: false),
                        GroupID = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateProcessed = c.DateTime(),
                        Status = c.String(maxLength: 32),
                        UserName = c.String(maxLength: 128),
                        dataloadErrors_DataLoadErrorsId = c.Long(),
                        DataLoadType_DataloadTypeID = c.Long(),
                        MassesToTerminate_Id = c.Long(),
                    })
                .PrimaryKey(t => t.DataLoadBatchId)
                .ForeignKey("dbo.t_DataLoadErrors", t => t.dataloadErrors_DataLoadErrorsId)
                .ForeignKey("dbo.t_DataLoadType", t => t.DataLoadType_DataloadTypeID)
                .ForeignKey("dbo.t_MassTermination", t => t.MassesToTerminate_Id)
                .Index(t => t.dataloadErrors_DataLoadErrorsId)
                .Index(t => t.DataLoadType_DataloadTypeID)
                .Index(t => t.MassesToTerminate_Id);
            
            CreateTable(
                "dbo.t_DataLoadErrors",
                c => new
                    {
                        DataLoadErrorsId = c.Long(nullable: false, identity: true),
                        ErrRecord = c.String(storeType: "xml"),
                        ErrDescription = c.String(),
                        DataLoadBatchId = c.Long(nullable: false),
                        DataLoadBatch_DataLoadBatchId = c.Long(),
                    })
                .PrimaryKey(t => t.DataLoadErrorsId)
                .ForeignKey("dbo.t_DataLoadBatch", t => t.DataLoadBatch_DataLoadBatchId)
                .Index(t => t.DataLoadBatch_DataLoadBatchId);
            
            CreateTable(
                "dbo.t_DataLoadType",
                c => new
                    {
                        DataloadTypeID = c.Long(nullable: false),
                        DataloadType = c.String(),
                        Batches_DataLoadBatchId = c.Long(),
                    })
                .PrimaryKey(t => t.DataloadTypeID)
                .ForeignKey("dbo.t_DataLoadBatch", t => t.Batches_DataLoadBatchId)
                .Index(t => t.Batches_DataLoadBatchId);
            
            CreateTable(
                "dbo.t_MassTermination",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DataLoadBatchId = c.Long(nullable: false),
                        EmployeeNumber = c.String(nullable: false, maxLength: 128),
                        TerminationDate = c.DateTime(nullable: false),
                        TerminationReason = c.String(nullable: false, maxLength: 512),
                        DataLoadBatch_DataLoadBatchId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.t_DataLoadBatch", t => t.DataLoadBatch_DataLoadBatchId)
                .Index(t => t.DataLoadBatch_DataLoadBatchId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.t_DataLoadBatch", "MassesToTerminate_Id", "dbo.t_MassTermination");
            DropForeignKey("dbo.t_MassTermination", "DataLoadBatch_DataLoadBatchId", "dbo.t_DataLoadBatch");
            DropForeignKey("dbo.t_DataLoadBatch", "DataLoadType_DataloadTypeID", "dbo.t_DataLoadType");
            DropForeignKey("dbo.t_DataLoadType", "Batches_DataLoadBatchId", "dbo.t_DataLoadBatch");
            DropForeignKey("dbo.t_DataLoadBatch", "dataloadErrors_DataLoadErrorsId", "dbo.t_DataLoadErrors");
            DropForeignKey("dbo.t_DataLoadErrors", "DataLoadBatch_DataLoadBatchId", "dbo.t_DataLoadBatch");
            DropIndex("dbo.t_MassTermination", new[] { "DataLoadBatch_DataLoadBatchId" });
            DropIndex("dbo.t_DataLoadType", new[] { "Batches_DataLoadBatchId" });
            DropIndex("dbo.t_DataLoadErrors", new[] { "DataLoadBatch_DataLoadBatchId" });
            DropIndex("dbo.t_DataLoadBatch", new[] { "MassesToTerminate_Id" });
            DropIndex("dbo.t_DataLoadBatch", new[] { "DataLoadType_DataloadTypeID" });
            DropIndex("dbo.t_DataLoadBatch", new[] { "dataloadErrors_DataLoadErrorsId" });
            DropTable("dbo.t_MassTermination");
            DropTable("dbo.t_DataLoadType");
            DropTable("dbo.t_DataLoadErrors");
            DropTable("dbo.t_DataLoadBatch");
        }
    }
}
