namespace WiGi.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddAboutFieldToPage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pages", "About", c => c.String(true, defaultValue: ""));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pages", "About");
        }
    }
}
