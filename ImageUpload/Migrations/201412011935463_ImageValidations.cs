namespace ImageUpload.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImageValidations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Images", "ImageTitle", c => c.String(nullable: false));
            AlterColumn("dbo.Images", "ImagePath", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Images", "ImagePath", c => c.String());
            AlterColumn("dbo.Images", "ImageTitle", c => c.String());
        }
    }
}
