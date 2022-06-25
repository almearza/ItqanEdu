namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedatetime : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.InstructorProfiles", "AppointedDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InstructorProfiles", "AppointedDate", c => c.DateTime(nullable: false));
        }
    }
}
