namespace ENETCareMVCApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DistrictIDadd3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Intervention", "InterventionType_InterventionTypeID", "dbo.InterventionType");
            DropIndex("dbo.Intervention", new[] { "InterventionType_InterventionTypeID" });
            RenameColumn(table: "dbo.Intervention", name: "Client_ClientID", newName: "ClientID");
            RenameColumn(table: "dbo.Intervention", name: "InterventionType_InterventionTypeID", newName: "InterventionTypeID");
            RenameIndex(table: "dbo.Intervention", name: "IX_Client_ClientID", newName: "IX_ClientID");
            AddColumn("dbo.Intervention", "UserID", c => c.Int(nullable: false));
            AddColumn("dbo.Intervention", "ApproveUserID", c => c.Int(nullable: false));
            AlterColumn("dbo.Intervention", "InterventionState", c => c.Int(nullable: false));
            AlterColumn("dbo.Intervention", "InterventionTypeID", c => c.Int(nullable: false));
            CreateIndex("dbo.Intervention", "InterventionTypeID");
            AddForeignKey("dbo.Intervention", "InterventionTypeID", "dbo.InterventionType", "InterventionTypeID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Intervention", "InterventionTypeID", "dbo.InterventionType");
            DropIndex("dbo.Intervention", new[] { "InterventionTypeID" });
            AlterColumn("dbo.Intervention", "InterventionTypeID", c => c.Int());
            AlterColumn("dbo.Intervention", "InterventionState", c => c.String(nullable: false));
            DropColumn("dbo.Intervention", "ApproveUserID");
            DropColumn("dbo.Intervention", "UserID");
            RenameIndex(table: "dbo.Intervention", name: "IX_ClientID", newName: "IX_Client_ClientID");
            RenameColumn(table: "dbo.Intervention", name: "InterventionTypeID", newName: "InterventionType_InterventionTypeID");
            RenameColumn(table: "dbo.Intervention", name: "ClientID", newName: "Client_ClientID");
            CreateIndex("dbo.Intervention", "InterventionType_InterventionTypeID");
            AddForeignKey("dbo.Intervention", "InterventionType_InterventionTypeID", "dbo.InterventionType", "InterventionTypeID");
        }
    }
}
