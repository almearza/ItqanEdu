namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeuser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InstructorProfiles", "IUserId", "dbo.AspNetUsers");
            DropIndex("dbo.InstructorProfiles", new[] { "IUserId" });
            AddColumn("dbo.InstructorProfiles", "UserName", c => c.String());
            DropColumn("dbo.InstructorProfiles", "IUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InstructorProfiles", "IUserId", c => c.String(maxLength: 128));
            DropColumn("dbo.InstructorProfiles", "UserName");
            CreateIndex("dbo.InstructorProfiles", "IUserId");
            AddForeignKey("dbo.InstructorProfiles", "IUserId", "dbo.AspNetUsers", "Id");
        }
    }
}
