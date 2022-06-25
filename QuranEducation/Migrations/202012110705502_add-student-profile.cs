namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addstudentprofile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StudentProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImgUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StudentProfiles");
        }
    }
}
