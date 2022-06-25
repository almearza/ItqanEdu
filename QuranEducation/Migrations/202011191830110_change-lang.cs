namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changelang : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Langs", "LangName", c => c.String(nullable: false));
            DropColumn("dbo.Langs", "LangCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Langs", "LangCode", c => c.String());
            DropColumn("dbo.Langs", "LangName");
        }
    }
}
