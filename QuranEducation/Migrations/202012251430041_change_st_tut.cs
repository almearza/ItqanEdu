namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_st_tut : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StudentTutorials", "AcceptDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StudentTutorials", "AcceptDate", c => c.DateTime(nullable: false));
        }
    }
}
