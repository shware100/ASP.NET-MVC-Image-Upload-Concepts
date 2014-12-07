using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Web;

namespace ImageUpload.Models
{
    public partial class Image
    {
        public int ID { get; set; }

        [Required]
        [Display (Name="Image Title")]
        public string ImageTitle { get; set; }

        [Display(Name = "File Name")]
        public string OriginalFileName { get; set; }

        [Display(Name = "Image File")]
        public string ImagePath { get; set; }
    }
    public class ImageUploadDBContext : DbContext
    {
        public DbSet<Image> Images { get; set; }
    }
}