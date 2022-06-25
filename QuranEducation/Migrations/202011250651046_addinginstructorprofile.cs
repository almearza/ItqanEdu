namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addinginstructorprofile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InstructorProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Desc = c.String(),
                        ImageUrl = c.String(),
                        AppointedDate = c.DateTime(nullable: false),
                        IUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.IUserId)
                .Index(t => t.IUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InstructorProfiles", "IUserId", "dbo.AspNetUsers");
            DropIndex("dbo.InstructorProfiles", new[] { "IUserId" });
            DropTable("dbo.InstructorProfiles");
        }
    }
}
