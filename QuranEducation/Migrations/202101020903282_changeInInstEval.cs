namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeInInstEval : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InstEvals", "TutorialId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InstEvals", "TutorialId");
        }
    }
}
