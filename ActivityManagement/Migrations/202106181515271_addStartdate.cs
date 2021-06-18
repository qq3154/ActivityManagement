namespace ActivityManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStartdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Courses", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Courses", "Description", c => c.String());
            DropColumn("dbo.Courses", "StartDate");
        }
    }
}
