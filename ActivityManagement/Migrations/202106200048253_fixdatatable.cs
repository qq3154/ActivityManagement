namespace ActivityManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixdatatable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Courses", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.UserInfoes", "FullName", c => c.String(nullable: false));
            AlterColumn("dbo.UserInfoes", "ProgrammingLanguage", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserInfoes", "ProgrammingLanguage", c => c.String());
            AlterColumn("dbo.UserInfoes", "FullName", c => c.String());
            AlterColumn("dbo.Courses", "Description", c => c.String());
        }
    }
}
