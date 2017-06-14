namespace Lifebuoy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class qwe : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UsersVotes", "UserName", c => c.String());
            DropColumn("dbo.UsersVotes", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UsersVotes", "UserId", c => c.String());
            DropColumn("dbo.UsersVotes", "UserName");
        }
    }
}
