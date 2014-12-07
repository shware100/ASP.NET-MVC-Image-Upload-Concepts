using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ImageUpload.Models
{
    public class ImageViewModel
    {

        public int ID;

        [Required]
        public string ImageTitle { get; set; }

        [PaperClip("~/Images", "100x20", "200x40", "640x480")]
        [DataType(DataType.Upload)]
        [Display(Name = "Image Upload")]
        public HttpPostedFileBase ImageUpload { get; set; }

        public string ImageDisplayUrl { get; set; }

        public string UniqueImageFileNameBase { get; set; }

        [Display(Name = "Image Path")]
        public string ImagePath { get; set; }

        [Display(Name = "File Name")]
        public string OriginalFileName { get; set; }
    }
}