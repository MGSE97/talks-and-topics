namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "weather.daily",
                c => new
                    {
                        Location = c.String(nullable: false, maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        Type = c.Int(nullable: false),
                        Temperature = c.Double(nullable: false),
                        Rain = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.Location, t.Date });
            
        }
        
        public override void Down()
        {
            DropTable("weather.daily");
        }
    }
}
