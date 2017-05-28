namespace ENETCareMVCApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Newdatabase : DbMigration
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
                        DistrictID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClientID)
                .ForeignKey("dbo.District", t => t.DistrictID)
                .Index(t => t.DistrictID);
            
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
                        LoginName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        UserType = c.String(nullable: false),
                        MaxHour = c.Int(nullable: false),
                        MaxCost = c.Int(nullable: false),
                        DistrictID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.District", t => t.DistrictID)
                .Index(t => t.DistrictID);
            
            CreateTable(
                "dbo.Intervention",
                c => new
                    {
                        InterventionID = c.Int(nullable: false, identity: true),
                        LabourRequired = c.Single(nullable: false),
                        CostRequired = c.Single(nullable: false),
                        InterventionDate = c.String(nullable: false),
                        InterventionState = c.Int(nullable: false),
                        Notes = c.String(),
                        RemainingLife = c.Int(),
                        LastEditDate = c.String(),
                        ClientID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        ApproveUserID = c.Int(nullable: false),
                        InterventionTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InterventionID)
                .ForeignKey("dbo.Client", t => t.ClientID)
                .ForeignKey("dbo.InterventionType", t => t.InterventionTypeID)
                .ForeignKey("dbo.User", t => t.UserID)
                .Index(t => t.ClientID)
                .Index(t => t.UserID)
                .Index(t => t.InterventionTypeID);
            
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
            DropForeignKey("dbo.User", "DistrictID", "dbo.District");
            DropForeignKey("dbo.Intervention", "UserID", "dbo.User");
            DropForeignKey("dbo.Intervention", "InterventionTypeID", "dbo.InterventionType");
            DropForeignKey("dbo.Intervention", "ClientID", "dbo.Client");
            DropForeignKey("dbo.Client", "DistrictID", "dbo.District");
            DropIndex("dbo.Intervention", new[] { "InterventionTypeID" });
            DropIndex("dbo.Intervention", new[] { "UserID" });
            DropIndex("dbo.Intervention", new[] { "ClientID" });
            DropIndex("dbo.User", new[] { "DistrictID" });
            DropIndex("dbo.Client", new[] { "DistrictID" });
            DropTable("dbo.InterventionType");
            DropTable("dbo.Intervention");
            DropTable("dbo.User");
            DropTable("dbo.District");
            DropTable("dbo.Client");
        }
    }
}
