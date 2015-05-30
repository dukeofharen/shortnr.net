namespace Shortnr.Web.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.short_urls",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        long_url = c.String(nullable: false, maxLength: 1000, storeType: "nvarchar"),
                        segment = c.String(nullable: false, maxLength: 20, storeType: "nvarchar"),
                        added = c.DateTime(nullable: false, precision: 0),
                        ip = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        num_of_clicks = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.stats",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        click_date = c.DateTime(nullable: false, precision: 0),
                        ip = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        referer = c.String(maxLength: 500, storeType: "nvarchar"),
                        shortUrl_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.short_urls", t => t.shortUrl_id, cascadeDelete: true)
                .Index(t => t.shortUrl_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.stats", "shortUrl_id", "dbo.short_urls");
            DropIndex("dbo.stats", new[] { "shortUrl_id" });
            DropTable("dbo.stats");
            DropTable("dbo.short_urls");
        }
    }
}
