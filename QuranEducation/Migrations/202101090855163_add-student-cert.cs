namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addstudentcert : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentTutorials", "Certified", c => c.Boolean(nullable: false));
            AddColumn("dbo.StudentTutorials", "CertifiedDate", c => c.DateTime());
            AddColumn("dbo.StudentTutorials", "CertifiedDegree", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentTutorials", "CertifiedDegree");
            DropColumn("dbo.StudentTutorials", "CertifiedDate");
            DropColumn("dbo.StudentTutorials", "Certified");
        }
    }
}
