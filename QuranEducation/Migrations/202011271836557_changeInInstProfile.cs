namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeInInstProfile : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.InstructorProfiles", "LangCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InstructorProfiles", "LangCode", c => c.String());
        }
    }
}
