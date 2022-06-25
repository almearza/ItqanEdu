namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCountOfLec : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tutorials", "CountOfLec", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tutorials", "CountOfLec");
        }
    }
}
