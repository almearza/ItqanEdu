namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addinglangcodetoinstructor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InstructorProfiles", "LangCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InstructorProfiles", "LangCode");
        }
    }
}
