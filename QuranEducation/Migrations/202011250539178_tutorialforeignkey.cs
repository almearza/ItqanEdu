namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tutorialforeignkey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tutorials", "AUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Tutorials", "AUserId");
            AddForeignKey("dbo.Tutorials", "AUserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tutorials", "AUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Tutorials", new[] { "AUserId" });
            DropColumn("dbo.Tutorials", "AUserId");
        }
    }
}
