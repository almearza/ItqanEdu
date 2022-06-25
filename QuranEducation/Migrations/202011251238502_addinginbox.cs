namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addinginbox : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inboxes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AUserId = c.String(maxLength: 128),
                        Message = c.String(),
                        Arrival = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AUserId)
                .Index(t => t.AUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inboxes", "AUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Inboxes", new[] { "AUserId" });
            DropTable("dbo.Inboxes");
        }
    }
}
