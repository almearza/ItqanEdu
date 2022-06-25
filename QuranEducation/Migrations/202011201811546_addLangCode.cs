namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addLangCode : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "LangId", "dbo.Langs");
            DropIndex("dbo.AspNetUsers", new[] { "LangId" });
            AddColumn("dbo.AspNetUsers", "LangCode", c => c.String());
            DropColumn("dbo.AspNetUsers", "LangId");
            DropTable("dbo.Langs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Langs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LangName = c.String(nullable: false),
                        Code = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "LangId", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "LangCode");
            CreateIndex("dbo.AspNetUsers", "LangId");
            AddForeignKey("dbo.AspNetUsers", "LangId", "dbo.Langs", "Id", cascadeDelete: true);
        }
    }
}
