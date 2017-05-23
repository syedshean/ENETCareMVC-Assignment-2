namespace ENETCareMVCApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequiredfield4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Intervention", "User_UserID", "dbo.User");
            DropIndex("dbo.Intervention", new[] { "User_UserID" });
            AlterColumn("dbo.Intervention", "User_UserID", c => c.Int());
            CreateIndex("dbo.Intervention", "User_UserID");
            AddForeignKey("dbo.Intervention", "User_UserID", "dbo.User", "UserID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Intervention", "User_UserID", "dbo.User");
            DropIndex("dbo.Intervention", new[] { "User_UserID" });
            AlterColumn("dbo.Intervention", "User_UserID", c => c.Int(nullable: false));
            CreateIndex("dbo.Intervention", "User_UserID");
            AddForeignKey("dbo.Intervention", "User_UserID", "dbo.User", "UserID", cascadeDelete: true);
        }
    }
}
