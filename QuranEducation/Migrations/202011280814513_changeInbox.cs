namespace QuranEducation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeInbox : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inboxes", "Sender", c => c.String());
            AddColumn("dbo.Inboxes", "Link", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inboxes", "Link");
            DropColumn("dbo.Inboxes", "Sender");
        }
    }
}
