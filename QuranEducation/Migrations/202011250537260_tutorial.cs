namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tutorial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tutorials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Descr = c.String(),
                        OpenDate = c.DateTime(nullable: false),
                        CloseDate = c.DateTime(nullable: false),
                        ImageUrl = c.String(),
                        LangCode = c.String(),
                        Active = c.Boolean(nullable: false),
                        DoneBy = c.String(),
                        StDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tutorials");
        }
    }
}
