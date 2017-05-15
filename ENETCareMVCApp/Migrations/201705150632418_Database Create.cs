namespace ENETCareMVCApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatabaseCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Client",
                c => new
                    {
                        ClientID = c.Int(nullable: false, identity: true),
                        ClientName = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        District_DistrictID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClientID)
                .ForeignKey("dbo.District", t => t.District_DistrictID, cascadeDelete: true)
                .Index(t => t.District_DistrictID);
            
            CreateTable(
                "dbo.District",
                c => new
                    {
                        DistrictID = c.Int(nullable: false, identity: true),
                        DistrictName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.DistrictID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false),
                        LoginName = c.Int(nullable: false),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        UserType = c.String(nullable: false),
                        MaxHour = c.Int(nullable: false),
                        MaxCost = c.Int(nullable: false),
                        District_DistrictID = c.Int(),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.District", t => t.District_DistrictID)
                .Index(t => t.District_DistrictID);
            
            CreateTable(
                "dbo.Intervention",
                c => new
                    {
                        InterventionID = c.Int(nullable: false, identity: true),
                        LabourRequired = c.Single(nullable: false),
                        CostRequired = c.Single(nullable: false),
                        InterventionDate = c.String(nullable: false),
                        InterventionState = c.String(nullable: false),
                        Notes = c.String(),
                        RemainingLife = c.Int(),
                        LastEditDate = c.String(),
                        ApprovalUser_UserID = c.Int(),
                        Client_ClientID = c.Int(nullable: false),
                        InterventionType_InterventionTypeID = c.Int(),
                        User_UserID = c.Int(nullable: false),
                        User_UserID1 = c.Int(),
                    })
                .PrimaryKey(t => t.InterventionID)
                .ForeignKey("dbo.User", t => t.ApprovalUser_UserID)
                .ForeignKey("dbo.Client", t => t.Client_ClientID, cascadeDelete: true)
                .ForeignKey("dbo.InterventionType", t => t.InterventionType_InterventionTypeID)
                .ForeignKey("dbo.User", t => t.User_UserID, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.User_UserID1)
                .Index(t => t.ApprovalUser_UserID)
                .Index(t => t.Client_ClientID)
                .Index(t => t.InterventionType_InterventionTypeID)
                .Index(t => t.User_UserID)
                .Index(t => t.User_UserID1);
            
            CreateTable(
                "dbo.InterventionType",
                c => new
                    {
                        InterventionTypeID = c.Int(nullable: false, identity: true),
                        InterventionTypeName = c.String(nullable: false),
                        EstimatedLabour = c.Double(nullable: false),
                        EstimatedCost = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.InterventionTypeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Client", "District_DistrictID", "dbo.District");
            DropForeignKey("dbo.Intervention", "User_UserID1", "dbo.User");
            DropForeignKey("dbo.Intervention", "User_UserID", "dbo.User");
            DropForeignKey("dbo.Intervention", "InterventionType_InterventionTypeID", "dbo.InterventionType");
            DropForeignKey("dbo.Intervention", "Client_ClientID", "dbo.Client");
            DropForeignKey("dbo.Intervention", "ApprovalUser_UserID", "dbo.User");
            DropForeignKey("dbo.User", "District_DistrictID", "dbo.District");
            DropIndex("dbo.Intervention", new[] { "User_UserID1" });
            DropIndex("dbo.Intervention", new[] { "User_UserID" });
            DropIndex("dbo.Intervention", new[] { "InterventionType_InterventionTypeID" });
            DropIndex("dbo.Intervention", new[] { "Client_ClientID" });
            DropIndex("dbo.Intervention", new[] { "ApprovalUser_UserID" });
            DropIndex("dbo.User", new[] { "District_DistrictID" });
            DropIndex("dbo.Client", new[] { "District_DistrictID" });
            DropTable("dbo.InterventionType");
            DropTable("dbo.Intervention");
            DropTable("dbo.User");
            DropTable("dbo.District");
            DropTable("dbo.Client");
        }
    }
}
