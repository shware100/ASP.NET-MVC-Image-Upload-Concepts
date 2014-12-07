namespace ImageUpload.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImageNotPathValidation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Images", "ImagePath", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Images", "ImagePath", c => c.String(nullable: false));
        }
    }
}
