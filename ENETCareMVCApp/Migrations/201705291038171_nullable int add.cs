namespace ENETCareMVCApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullableintadd : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.User", "MaxHour", c => c.Int());
            AlterColumn("dbo.User", "MaxCost", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User", "MaxCost", c => c.Int(nullable: false));
            AlterColumn("dbo.User", "MaxHour", c => c.Int(nullable: false));
        }
    }
}
