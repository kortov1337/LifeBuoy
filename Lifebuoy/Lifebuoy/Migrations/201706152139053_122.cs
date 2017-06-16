namespace Lifebuoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _122 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reviews", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reviews", "UserId");
        }
    }
}
