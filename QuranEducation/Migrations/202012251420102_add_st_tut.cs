namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_st_tut : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StudentTutorials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TutorialId = c.Int(nullable: false),
                        UserName = c.String(),
                        ManagemetAccept = c.Boolean(nullable: false),
                        RequestDate = c.DateTime(nullable: false),
                        AcceptDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tutorials", t => t.TutorialId, cascadeDelete: true)
                .Index(t => t.TutorialId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentTutorials", "TutorialId", "dbo.Tutorials");
            DropIndex("dbo.StudentTutorials", new[] { "TutorialId" });
            DropTable("dbo.StudentTutorials");
        }
    }
}
