namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedatatstprofile : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StudentProfiles", "BOD", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StudentProfiles", "BOD", c => c.DateTime(nullable: false));
        }
    }
}
