namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addsol : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssSolutions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AssignmentId = c.Int(nullable: false),
                        UserName = c.String(),
                        Descr = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assigments", t => t.AssignmentId, cascadeDelete: true)
                .Index(t => t.AssignmentId);
            
            CreateTable(
                "dbo.SolutionAttachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SolutionId = c.Int(nullable: false),
                        FileUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssSolutions", t => t.SolutionId, cascadeDelete: true)
                .Index(t => t.SolutionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SolutionAttachments", "SolutionId", "dbo.AssSolutions");
            DropForeignKey("dbo.AssSolutions", "AssignmentId", "dbo.Assigments");
            DropIndex("dbo.SolutionAttachments", new[] { "SolutionId" });
            DropIndex("dbo.AssSolutions", new[] { "AssignmentId" });
            DropTable("dbo.SolutionAttachments");
            DropTable("dbo.AssSolutions");
        }
    }
}
