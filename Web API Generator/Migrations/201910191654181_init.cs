namespace Web_API_Generator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DataAnnotationViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.Int(nullable: false),
                        Param1 = c.String(),
                        Param2 = c.String(),
                        ErrorMessage = c.String(),
                        DataFieldId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DataFieldViewModels", t => t.DataFieldId, cascadeDelete: true)
                .Index(t => t.DataFieldId);
            
            CreateTable(
                "dbo.DataFieldViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.Int(nullable: false),
                        Lengh = c.String(),
                        Nullable = c.Boolean(nullable: false),
                        TableId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TableViewModels", t => t.TableId, cascadeDelete: true)
                .Index(t => t.TableId);
            
            CreateTable(
                "dbo.TableViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        DataFieldType = c.String(),
                        ProjectID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProjectViewModels", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.DataFieldForeignKeyViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Required = c.Boolean(nullable: false),
                        DataFieldViewID = c.Int(),
                        TableId = c.Int(),
                        TableViewModels_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TableViewModels", t => t.DataFieldViewID)
                .ForeignKey("dbo.TableViewModels", t => t.TableId)
                .ForeignKey("dbo.TableViewModels", t => t.TableViewModels_Id)
                .Index(t => t.DataFieldViewID)
                .Index(t => t.TableId)
                .Index(t => t.TableViewModels_Id);
            
            CreateTable(
                "dbo.ProjectViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Version = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EnumDetailsViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        EnumViewModels_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EnumViewModels", t => t.EnumViewModels_Id)
                .Index(t => t.EnumViewModels_Id);
            
            CreateTable(
                "dbo.EnumViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Descriptions = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.EnumDetailsViewModels", "EnumViewModels_Id", "dbo.EnumViewModels");
            DropForeignKey("dbo.DataAnnotationViewModels", "DataFieldId", "dbo.DataFieldViewModels");
            DropForeignKey("dbo.DataFieldViewModels", "TableId", "dbo.TableViewModels");
            DropForeignKey("dbo.TableViewModels", "ProjectID", "dbo.ProjectViewModels");
            DropForeignKey("dbo.DataFieldForeignKeyViewModels", "TableViewModels_Id", "dbo.TableViewModels");
            DropForeignKey("dbo.DataFieldForeignKeyViewModels", "TableId", "dbo.TableViewModels");
            DropForeignKey("dbo.DataFieldForeignKeyViewModels", "DataFieldViewID", "dbo.TableViewModels");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.EnumDetailsViewModels", new[] { "EnumViewModels_Id" });
            DropIndex("dbo.DataFieldForeignKeyViewModels", new[] { "TableViewModels_Id" });
            DropIndex("dbo.DataFieldForeignKeyViewModels", new[] { "TableId" });
            DropIndex("dbo.DataFieldForeignKeyViewModels", new[] { "DataFieldViewID" });
            DropIndex("dbo.TableViewModels", new[] { "ProjectID" });
            DropIndex("dbo.DataFieldViewModels", new[] { "TableId" });
            DropIndex("dbo.DataAnnotationViewModels", new[] { "DataFieldId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.EnumViewModels");
            DropTable("dbo.EnumDetailsViewModels");
            DropTable("dbo.ProjectViewModels");
            DropTable("dbo.DataFieldForeignKeyViewModels");
            DropTable("dbo.TableViewModels");
            DropTable("dbo.DataFieldViewModels");
            DropTable("dbo.DataAnnotationViewModels");
        }
    }
}
