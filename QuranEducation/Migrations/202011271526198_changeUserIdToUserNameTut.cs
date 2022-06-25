namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeUserIdToUserNameTut : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inboxes", "AUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tutorials", "AUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Inboxes", new[] { "AUserId" });
            DropIndex("dbo.Tutorials", new[] { "AUserId" });
            AddColumn("dbo.Inboxes", "AUserName", c => c.String());
            AddColumn("dbo.Tutorials", "InstUName", c => c.String());
            DropColumn("dbo.Inboxes", "AUserId");
            DropColumn("dbo.Tutorials", "AUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tutorials", "AUserId", c => c.String(maxLength: 128));
            AddColumn("dbo.Inboxes", "AUserId", c => c.String(maxLength: 128));
            DropColumn("dbo.Tutorials", "InstUName");
            DropColumn("dbo.Inboxes", "AUserName");
            CreateIndex("dbo.Tutorials", "AUserId");
            CreateIndex("dbo.Inboxes", "AUserId");
            AddForeignKey("dbo.Tutorials", "AUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Inboxes", "AUserId", "dbo.AspNetUsers", "Id");
        }
    }
}
