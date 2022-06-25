namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addlec : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Lectures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Descr = c.String(),
                        LecTime = c.DateTime(nullable: false),
                        VirtualRoomUrl = c.String(),
                        VideoUrl = c.String(),
                        AttachmentUrl = c.String(),
                        TutorialId = c.Int(nullable: false),
                        StDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tutorials", t => t.TutorialId, cascadeDelete: true)
                .Index(t => t.TutorialId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Lectures", "TutorialId", "dbo.Tutorials");
            DropIndex("dbo.Lectures", new[] { "TutorialId" });
            DropTable("dbo.Lectures");
        }
    }
}
