using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using ImageUpload.Models;

public class PaperClipModelBinder : DefaultModelBinder
{
    // core functionality http://stackoverflow.com/questions/5013824/paperclip-for-asp-net-mvc
    // modified to generate unique file names and passed the unique file name base back to calling controller
    public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
        if (!bindingContext.ModelMetadata.AdditionalValues.ContainsKey("PaperClip"))
        {
            return base.BindModel(controllerContext, bindingContext);
        }
        var paperClip = bindingContext.ModelMetadata.AdditionalValues["PaperClip"] as PaperClipAttribute;
        if (paperClip == null)
        {
            return base.BindModel(controllerContext, bindingContext);
        }
        
        // var uploadedFile = base.BindModel(controllerContext, bindingContext) as HttpPostedFileBase;
        var uploadedFile = base.BindModel(controllerContext, bindingContext) as HttpPostedFileBase;
        if (uploadedFile == null)
        {
            // bindingContext.ModelState.AddModelError("ImageUpload", "This field is required");
            return null;
        }

        var uploadPath = controllerContext.HttpContext.Server.MapPath(paperClip.UploadPath);
        if (!Directory.Exists(uploadPath))
        {
            throw new ArgumentException(string.Format("The specified folder \"{0}\" does not exist", uploadPath));
        }

        var validImageTypes = new string[]
            {
              "image/gif",
              "image/jpeg",
              "image/pjpeg",
              "image/png"
            };

        if (uploadedFile == null || uploadedFile.ContentLength == 0)
        {
            bindingContext.ModelState.AddModelError("ImageUpload", "This field is required");
        }
        else if (!validImageTypes.Contains(uploadedFile.ContentType))
        {
            bindingContext.ModelState.AddModelError("ImageUpload", "Please choose either a GIF, JPG or PNG image.");
        }

        // get out if things aren't right
        if (!bindingContext.ModelState.IsValid)
            return base.BindModel(controllerContext, bindingContext);

        // get different image sizes from model attributes
        var sizes =
            (from size in paperClip.Sizes
             let tokens = size.Split('x')
             select new Size(int.Parse(tokens[0]), int.Parse(tokens[1]))
            ).ToArray();

        // get unique file name 
        var uniqueFileName = Path.GetFileNameWithoutExtension(System.Guid.NewGuid().ToString());

        // get file extension so we can validate image type
        var extension = Path.GetExtension(uploadedFile.FileName);

        // save original file contents
        var origFileName = Path.Combine(uploadPath, string.Format("{0}{1}", uniqueFileName, extension));
        uploadedFile.SaveAs(origFileName);

        // iterate through different image sizes and save them to upload dir
        foreach (var size in sizes)
        {
            var fileName = Path.GetFileNameWithoutExtension(uploadedFile.FileName);
            
            var outputFilename = Path.Combine(
                uploadPath,
                string.Format("{0}-{1}x{2}{3}", uniqueFileName, size.Width, size.Height, extension)
            );
            Resize(uploadedFile.InputStream, outputFilename, size);
        }
        
        ImageViewModel imv = (ImageViewModel)bindingContext.ModelMetadata.Container;
        imv.OriginalFileName = uploadedFile.FileName;
        imv.UniqueImageFileNameBase = uniqueFileName;
        imv.ImagePath = string.Format("{0}/{1}", paperClip.UploadPath, Path.GetFileName(origFileName));

        return base.BindModel(controllerContext, bindingContext);
    }

    private void Resize(Stream input, string outputFile, Size size)
    {
        using (var image = System.Drawing.Image.FromStream(input))
        using (var bmp = new Bitmap(size.Width, size.Height))
        using (var gr = Graphics.FromImage(bmp))
        {
            gr.CompositingQuality = CompositingQuality.HighSpeed;
            gr.SmoothingMode = SmoothingMode.HighSpeed;
            gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
            gr.DrawImage(image, new Rectangle(0, 0, size.Width, size.Height));
            bmp.Save(outputFile);
        }
    }
}
