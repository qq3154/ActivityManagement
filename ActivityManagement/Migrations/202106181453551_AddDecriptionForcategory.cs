namespace ActivityManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDecriptionForcategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "Description", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "Description");
        }
    }
}
