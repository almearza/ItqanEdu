namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStrartTime2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assigments", "StartTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Assigments", "StrartTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Assigments", "StrartTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Assigments", "StartTime");
        }
    }
}
