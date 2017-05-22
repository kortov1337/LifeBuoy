namespace Lifebuoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class qwe : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "Prices", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Offers", "Prices");
        }
    }
}
