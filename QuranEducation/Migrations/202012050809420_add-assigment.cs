namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addassigment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AssigmentId = c.Int(nullable: false),
                        StudentId = c.String(maxLength: 128),
                        Descr = c.String(),
                        Degree = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assigments", t => t.AssigmentId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.StudentId)
                .Index(t => t.AssigmentId)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.Assigments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Descr = c.String(),
                        StrartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        TutorialId = c.Int(nullable: false),
                        Degree = c.Double(nullable: false),
                        StDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tutorials", t => t.TutorialId, cascadeDelete: true)
                .Index(t => t.TutorialId);
            
            CreateTable(
                "dbo.AssigmentAttachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AssigmentId = c.Int(nullable: false),
                        FileUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assigments", t => t.AssigmentId, cascadeDelete: true)
                .Index(t => t.AssigmentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Answers", "StudentId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Answers", "AssigmentId", "dbo.Assigments");
            DropForeignKey("dbo.Assigments", "TutorialId", "dbo.Tutorials");
            DropForeignKey("dbo.AssigmentAttachments", "AssigmentId", "dbo.Assigments");
            DropIndex("dbo.AssigmentAttachments", new[] { "AssigmentId" });
            DropIndex("dbo.Assigments", new[] { "TutorialId" });
            DropIndex("dbo.Answers", new[] { "StudentId" });
            DropIndex("dbo.Answers", new[] { "AssigmentId" });
            DropTable("dbo.AssigmentAttachments");
            DropTable("dbo.Assigments");
            DropTable("dbo.Answers");
        }
    }
}
