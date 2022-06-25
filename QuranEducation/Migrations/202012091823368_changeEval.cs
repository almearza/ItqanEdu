namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeEval : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InstructorProfiles", "EvalId", c => c.Int(nullable: false));
            DropColumn("dbo.InstructorProfiles", "Eval");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InstructorProfiles", "Eval", c => c.String());
            DropColumn("dbo.InstructorProfiles", "EvalId");
        }
    }
}
