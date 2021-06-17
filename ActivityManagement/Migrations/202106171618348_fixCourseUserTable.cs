namespace ActivityManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixCourseUserTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CourseUsers", "TeamId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CourseUsers", "TeamId", c => c.Int());
        }
    }
}
