namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addInstEval : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InstEvals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InstUserName = c.String(nullable: false),
                        EvalUserName = c.String(),
                        ArriveInATime = c.Boolean(nullable: false),
                        GoodInStudy = c.Boolean(nullable: false),
                        GoodCommunications = c.Boolean(nullable: false),
                        GoodInVoice = c.Boolean(nullable: false),
                        TechProblemInTutorial = c.Boolean(nullable: false),
                        GoodInUsingBoard = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.InstEvals");
        }
    }
}
