namespace Lifebuoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class qww1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProvidersOffers", "OfferId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProvidersOffers", "OfferId", c => c.String());
        }
    }
}
