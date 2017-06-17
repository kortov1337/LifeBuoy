namespace Lifebuoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class qwe : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "hotOffer", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Offers", "hotOffer");
        }
    }
}
