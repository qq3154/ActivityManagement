namespace ActivityManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CourseUsers",
                c => new
                    {
                        CourseId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.CourseId, t.UserId })
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.CourseId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserInfoes",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(),
                        Age = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserInfoes", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CourseUsers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CourseUsers", "CourseId", "dbo.Courses");
            DropIndex("dbo.UserInfoes", new[] { "UserId" });
            DropIndex("dbo.CourseUsers", new[] { "UserId" });
            DropIndex("dbo.CourseUsers", new[] { "CourseId" });
            DropTable("dbo.UserInfoes");
            DropTable("dbo.CourseUsers");
            DropTable("dbo.Categories");
        }
    }
}
