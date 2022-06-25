namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCodeLang : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Langs", "Code", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Langs", "Code");
        }
    }
}
