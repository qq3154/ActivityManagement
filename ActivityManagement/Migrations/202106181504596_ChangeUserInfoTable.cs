namespace ActivityManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUserInfoTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserInfoes", "TOEICScore", c => c.Int(nullable: false));
            AddColumn("dbo.UserInfoes", "ProgrammingLanguage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserInfoes", "ProgrammingLanguage");
            DropColumn("dbo.UserInfoes", "TOEICScore");
        }
    }
}
