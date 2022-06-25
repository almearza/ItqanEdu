namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStudentProfiles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentProfiles", "AppUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.StudentProfiles", "AppUserId");
            AddForeignKey("dbo.StudentProfiles", "AppUserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentProfiles", "AppUserId", "dbo.AspNetUsers");
            DropIndex("dbo.StudentProfiles", new[] { "AppUserId" });
            DropColumn("dbo.StudentProfiles", "AppUserId");
        }
    }
}
