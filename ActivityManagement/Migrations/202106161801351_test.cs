namespace ActivityManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourseUsers", "TeamId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CourseUsers", "TeamId");
        }
    }
}
