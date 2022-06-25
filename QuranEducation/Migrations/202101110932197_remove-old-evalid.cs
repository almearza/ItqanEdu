namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeoldevalid : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.InstructorProfiles", "EvalId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InstructorProfiles", "EvalId", c => c.Int(nullable: false));
        }
    }
}
