namespace WiGi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDocIdToPages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pages", "DocId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pages", "DocId");
        }
    }
}
