namespace WiGi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Username = c.String(),
                        Password = c.String(),
                        IsAdmin = c.Boolean(nullable: true, defaultValue: false, defaultValueSql: "0"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Url = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TagPages",
                c => new
                    {
                        Tag_Id = c.Int(nullable: false),
                        Page_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.Page_Id })
                .ForeignKey("dbo.Tags", t => t.Tag_Id, cascadeDelete: true)
                .ForeignKey("dbo.Pages", t => t.Page_Id, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => t.Page_Id);
            
            CreateTable(
                "dbo.CategoryPages",
                c => new
                    {
                        Category_Id = c.Int(nullable: false),
                        Page_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Category_Id, t.Page_Id })
                .ForeignKey("dbo.Categories", t => t.Category_Id, cascadeDelete: true)
                .ForeignKey("dbo.Pages", t => t.Page_Id, cascadeDelete: true)
                .Index(t => t.Category_Id)
                .Index(t => t.Page_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.CategoryPages", new[] { "Page_Id" });
            DropIndex("dbo.CategoryPages", new[] { "Category_Id" });
            DropIndex("dbo.TagPages", new[] { "Page_Id" });
            DropIndex("dbo.TagPages", new[] { "Tag_Id" });
            DropForeignKey("dbo.CategoryPages", "Page_Id", "dbo.Pages");
            DropForeignKey("dbo.CategoryPages", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.TagPages", "Page_Id", "dbo.Pages");
            DropForeignKey("dbo.TagPages", "Tag_Id", "dbo.Tags");
            DropTable("dbo.CategoryPages");
            DropTable("dbo.TagPages");
            DropTable("dbo.Categories");
            DropTable("dbo.Tags");
            DropTable("dbo.Pages");
            DropTable("dbo.Users");
        }
    }
}
