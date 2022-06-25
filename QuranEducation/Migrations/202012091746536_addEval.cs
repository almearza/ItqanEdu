namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addEval : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InstructorProfiles", "Eval", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InstructorProfiles", "Eval");
        }
    }
}
