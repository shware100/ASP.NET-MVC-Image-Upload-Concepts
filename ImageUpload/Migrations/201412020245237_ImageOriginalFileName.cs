namespace ImageUpload.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImageOriginalFileName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Images", "OriginalFileName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Images", "OriginalFileName");
        }
    }
}
