namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changelec : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lectures", "LecStrartTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Lectures", "LecEndTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Lectures", "LecTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Lectures", "LecTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Lectures", "LecEndTime");
            DropColumn("dbo.Lectures", "LecStrartTime");
        }
    }
}
