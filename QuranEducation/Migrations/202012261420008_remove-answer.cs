namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeanswer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AnswerAttachments", "AnswerId", "dbo.Answers");
            DropForeignKey("dbo.Answers", "AssigmentId", "dbo.Assigments");
            DropForeignKey("dbo.Answers", "StudentId", "dbo.AspNetUsers");
            DropIndex("dbo.Answers", new[] { "AssigmentId" });
            DropIndex("dbo.Answers", new[] { "StudentId" });
            DropIndex("dbo.AnswerAttachments", new[] { "AnswerId" });
            DropTable("dbo.Answers");
            DropTable("dbo.AnswerAttachments");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AnswerAttachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AnswerId = c.Int(nullable: false),
                        FileUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.AnswerAttachments", "AnswerId");
            CreateIndex("dbo.Answers", "StudentId");
            CreateIndex("dbo.Answers", "AssigmentId");
            AddForeignKey("dbo.Answers", "StudentId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Answers", "AssigmentId", "dbo.Assigments", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AnswerAttachments", "AnswerId", "dbo.Answers", "Id", cascadeDelete: true);
        }
    }
}
