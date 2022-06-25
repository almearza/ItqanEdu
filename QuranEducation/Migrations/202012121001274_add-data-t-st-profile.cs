namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adddatatstprofile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentProfiles", "EnName", c => c.String());
            AddColumn("dbo.StudentProfiles", "Nationality", c => c.String());
            AddColumn("dbo.StudentProfiles", "BOD", c => c.DateTime(nullable: false));
            AddColumn("dbo.StudentProfiles", "HomePhoneNumber", c => c.String());
            AddColumn("dbo.StudentProfiles", "ResidentCountryCode", c => c.Int(nullable: false));
            AddColumn("dbo.StudentProfiles", "EduYear", c => c.String());
            AddColumn("dbo.StudentProfiles", "StScore", c => c.String());
            AddColumn("dbo.StudentProfiles", "CertUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentProfiles", "CertUrl");
            DropColumn("dbo.StudentProfiles", "StScore");
            DropColumn("dbo.StudentProfiles", "EduYear");
            DropColumn("dbo.StudentProfiles", "ResidentCountryCode");
            DropColumn("dbo.StudentProfiles", "HomePhoneNumber");
            DropColumn("dbo.StudentProfiles", "BOD");
            DropColumn("dbo.StudentProfiles", "Nationality");
            DropColumn("dbo.StudentProfiles", "EnName");
        }
    }
}
