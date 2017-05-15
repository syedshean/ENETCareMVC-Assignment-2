namespace ENETCareMVCApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Databaseupdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.User", "LoginName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User", "LoginName", c => c.Int(nullable: false));
        }
    }
}
