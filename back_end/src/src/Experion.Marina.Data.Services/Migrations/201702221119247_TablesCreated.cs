namespace Experion.Marina.Data.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TablesCreated : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuditEntries",
                c => new
                    {
                        AuditEntryID = c.Int(nullable: false, identity: true),
                        EntitySetName = c.String(maxLength: 255),
                        EntityTypeName = c.String(maxLength: 255),
                        State = c.Int(nullable: false),
                        StateName = c.String(maxLength: 255),
                        CreatedBy = c.String(maxLength: 255),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AuditEntryID);
            
            CreateTable(
                "dbo.AuditEntryProperties",
                c => new
                    {
                        AuditEntryPropertyID = c.Int(nullable: false, identity: true),
                        AuditEntryID = c.Int(nullable: false),
                        RelationName = c.String(maxLength: 255),
                        PropertyName = c.String(maxLength: 255),
                        OldValue = c.String(),
                        NewValue = c.String(),
                    })
                .PrimaryKey(t => t.AuditEntryPropertyID)
                .ForeignKey("dbo.AuditEntries", t => t.AuditEntryID, cascadeDelete: true)
                .Index(t => t.AuditEntryID);
            
            CreateTable(
                "dbo.SampleEntities",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Templates",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TemplateName = c.String(),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserCredentials", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserCredentials",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserName = c.String(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TemplateDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TemplateId = c.Long(nullable: false),
                        Label = c.String(nullable: false),
                        ControlType = c.String(nullable: false),
                        IsReadOnly = c.Boolean(nullable: false),
                        IsVisible = c.Boolean(nullable: false),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Templates", t => t.TemplateId, cascadeDelete: true)
                .Index(t => t.TemplateId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TemplateDetails", "TemplateId", "dbo.Templates");
            DropForeignKey("dbo.Templates", "UserId", "dbo.UserCredentials");
            DropForeignKey("dbo.AuditEntryProperties", "AuditEntryID", "dbo.AuditEntries");
            DropIndex("dbo.TemplateDetails", new[] { "TemplateId" });
            DropIndex("dbo.Templates", new[] { "UserId" });
            DropIndex("dbo.AuditEntryProperties", new[] { "AuditEntryID" });
            DropTable("dbo.TemplateDetails");
            DropTable("dbo.UserCredentials");
            DropTable("dbo.Templates");
            DropTable("dbo.SampleEntities");
            DropTable("dbo.AuditEntryProperties");
            DropTable("dbo.AuditEntries");
        }
    }
}
