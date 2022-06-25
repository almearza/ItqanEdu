namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnswerAttachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AnswerId = c.Int(nullable: false),
                        FileUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Answers", t => t.AnswerId, cascadeDelete: true)
                .Index(t => t.AnswerId);
            
            AddColumn("dbo.Assigments", "AssType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AnswerAttachments", "AnswerId", "dbo.Answers");
            DropIndex("dbo.AnswerAttachments", new[] { "AnswerId" });
            DropColumn("dbo.Assigments", "AssType");
            DropTable("dbo.AnswerAttachments");
        }
    }
}
