namespace ImageUpload.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImageTitle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Images", "ImageTitle", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Images", "ImageTitle");
        }
    }
}
