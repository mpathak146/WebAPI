namespace Fourth.DataLoads.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSetup : DbMigration
    {
        /// <summary>
        /// Setting cascade delete to false
        /// </summary>
        public override void Up()
        {
            

            CreateTable(
                "dbo.t_DataLoad",
                c => new
                    {
                        DataLoadJobId = c.Guid(nullable: false),
                        DataLoadBatchId = c.Guid(nullable: false),
                        DataloadTypeRefID = c.Long(nullable: false),
                        GroupID = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateProcessed = c.DateTime(),
                        Status = c.String(maxLength: 32),
                        UserName = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.DataLoadJobId, t.DataLoadBatchId })
                .ForeignKey("dbo.t_DataLoadType", t => t.DataloadTypeRefID, cascadeDelete: false)
                .Index(t => t.DataloadTypeRefID);
            
            CreateTable(
                "dbo.t_DataLoadErrors",
                c => new
                    {
                        DataLoadErrorsId = c.Long(nullable: false, identity: true),
                        DataLoadJobRefId = c.Guid(nullable: false),
                        DataLoadBatchRefId = c.Guid(nullable: false),
                        ErrRecord = c.String(storeType: "xml"),
                        ErrDescription = c.String(),
                    })
                .PrimaryKey(t => t.DataLoadErrorsId)
                .ForeignKey("dbo.t_DataLoad", t => new { t.DataLoadJobRefId, t.DataLoadBatchRefId }, cascadeDelete: false)
                .Index(t => new { t.DataLoadJobRefId, t.DataLoadBatchRefId });
            
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
                        MassTerminationId = c.Long(nullable: false, identity: true),
                        DataLoadJobRefId = c.Guid(nullable: false),
                        DataLoadBatchRefId = c.Guid(nullable: false),
                        EmployeeNumber = c.String(nullable: false, maxLength: 128),
                        TerminationDate = c.DateTime(nullable: false),
                        TerminationReason = c.String(nullable: false, maxLength: 512),
                    })
                .PrimaryKey(t => t.MassTerminationId)
                .ForeignKey("dbo.t_DataLoad", t => new { t.DataLoadJobRefId, t.DataLoadBatchRefId }, cascadeDelete: false)
                .Index(t => new { t.DataLoadJobRefId, t.DataLoadBatchRefId });
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.t_MassTermination", new[] { "DataLoadJobRefId", "DataLoadBatchRefId" }, "dbo.t_DataLoad");
            DropForeignKey("dbo.t_DataLoad", "DataloadTypeRefID", "dbo.t_DataLoadType");
            DropForeignKey("dbo.t_DataLoadErrors", new[] { "DataLoadJobRefId", "DataLoadBatchRefId" }, "dbo.t_DataLoad");
            DropIndex("dbo.t_MassTermination", new[] { "DataLoadJobRefId", "DataLoadBatchRefId" });
            DropIndex("dbo.t_DataLoadErrors", new[] { "DataLoadJobRefId", "DataLoadBatchRefId" });
            DropIndex("dbo.t_DataLoad", new[] { "DataloadTypeRefID" });
            DropTable("dbo.t_MassTermination");
            DropTable("dbo.t_DataLoadType");
            DropTable("dbo.t_DataLoadErrors");
            DropTable("dbo.t_DataLoad");
        }
    }
}
