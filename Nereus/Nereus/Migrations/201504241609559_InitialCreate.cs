namespace Nereus.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectQueries",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        Text = c.String(nullable: false),
                        LastUsed = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsPrivate = c.Boolean(nullable: false),
                        IsArchived = c.Boolean(nullable: false),
                        SearchProviderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SearchProviders", t => t.SearchProviderId, cascadeDelete: true)
                .Index(t => t.SearchProviderId);
            
            CreateTable(
                "dbo.SearchProviders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DisplayName = c.String(),
                        Description = c.String(),
                        ClassName = c.String(),
                        ConstructorParam = c.String(),
                        SupportsDateConstraints = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectUrlPatterns",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        Pattern = c.String(nullable: false),
                        Hidden = c.Boolean(nullable: false),
                        Irrelevant = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.ProjectUrls",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Hidden = c.Boolean(nullable: false),
                        Rating = c.Int(nullable: false),
                        UrlId = c.Long(nullable: false),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.Urls", t => t.UrlId, cascadeDelete: true)
                .Index(t => t.UrlId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.Urls",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Path = c.String(nullable: false, maxLength: 1024),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SearchProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SearchPeriod_SearchPeriodOption = c.Int(nullable: false),
                        SearchPeriod_StartDate = c.DateTime(),
                        ExcludePathsJson = c.String(),
                        IncludePathsJson = c.String(),
                        ExcludeQueriesJson = c.String(),
                        IncludeQueriesJson = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserProjectUrls",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProjectUrlId = c.Long(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProjectUrls", t => t.ProjectUrlId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ProjectUrlId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserSearchProfiles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        SearchProfileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SearchProfiles", t => t.SearchProfileId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.SearchProfileId);
            
            CreateTable(
                "dbo.UserSearchSettings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        NumResultsPerPage = c.Int(nullable: false),
                        ResultsColumnWidth = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserUISettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        TooltipsOn = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserUrls",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        UrlId = c.Long(nullable: false),
                        LastViewed = c.DateTime(),
                        PrevLastViewed = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Urls", t => t.UrlId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.UrlId);
            
            CreateTable(
                "dbo.ProjectUsers",
                c => new
                    {
                        Project_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.User_Id })
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Project_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserUrls", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserUrls", "UrlId", "dbo.Urls");
            DropForeignKey("dbo.UserUISettings", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserSearchSettings", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserSearchProfiles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserSearchProfiles", "SearchProfileId", "dbo.SearchProfiles");
            DropForeignKey("dbo.UserProjectUrls", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserProjectUrls", "ProjectUrlId", "dbo.ProjectUrls");
            DropForeignKey("dbo.ProjectUrls", "UrlId", "dbo.Urls");
            DropForeignKey("dbo.ProjectUrls", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.ProjectUrlPatterns", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.ProjectQueries", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.ProjectUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ProjectUsers", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Projects", "SearchProviderId", "dbo.SearchProviders");
            DropIndex("dbo.ProjectUsers", new[] { "User_Id" });
            DropIndex("dbo.ProjectUsers", new[] { "Project_Id" });
            DropIndex("dbo.UserUrls", new[] { "UrlId" });
            DropIndex("dbo.UserUrls", new[] { "UserId" });
            DropIndex("dbo.UserUISettings", new[] { "UserId" });
            DropIndex("dbo.UserSearchSettings", new[] { "UserId" });
            DropIndex("dbo.UserSearchProfiles", new[] { "SearchProfileId" });
            DropIndex("dbo.UserSearchProfiles", new[] { "UserId" });
            DropIndex("dbo.UserProjectUrls", new[] { "UserId" });
            DropIndex("dbo.UserProjectUrls", new[] { "ProjectUrlId" });
            DropIndex("dbo.ProjectUrls", new[] { "ProjectId" });
            DropIndex("dbo.ProjectUrls", new[] { "UrlId" });
            DropIndex("dbo.ProjectUrlPatterns", new[] { "ProjectId" });
            DropIndex("dbo.Projects", new[] { "SearchProviderId" });
            DropIndex("dbo.ProjectQueries", new[] { "ProjectId" });
            DropTable("dbo.ProjectUsers");
            DropTable("dbo.UserUrls");
            DropTable("dbo.UserUISettings");
            DropTable("dbo.UserSearchSettings");
            DropTable("dbo.UserSearchProfiles");
            DropTable("dbo.UserProjectUrls");
            DropTable("dbo.SearchProfiles");
            DropTable("dbo.Urls");
            DropTable("dbo.ProjectUrls");
            DropTable("dbo.ProjectUrlPatterns");
            DropTable("dbo.Users");
            DropTable("dbo.SearchProviders");
            DropTable("dbo.Projects");
            DropTable("dbo.ProjectQueries");
        }
    }
}
