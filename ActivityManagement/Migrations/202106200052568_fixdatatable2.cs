namespace ActivityManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixdatatable2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserInfoes", "ProgrammingLanguage", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserInfoes", "ProgrammingLanguage", c => c.String(nullable: false));
        }
    }
}
