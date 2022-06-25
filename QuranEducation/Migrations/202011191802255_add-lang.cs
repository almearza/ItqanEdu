namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addlang : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Langs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LangCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "LangId", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "LangId");
            AddForeignKey("dbo.AspNetUsers", "LangId", "dbo.Langs", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "LangId", "dbo.Langs");
            DropIndex("dbo.AspNetUsers", new[] { "LangId" });
            DropColumn("dbo.AspNetUsers", "LangId");
            DropTable("dbo.Langs");
        }
    }
}
