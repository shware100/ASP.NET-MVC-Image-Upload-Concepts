using ImageUpload.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ImageUpload.Controllers
{
    public class ImageController : Controller
    {
        private ImageUploadDBContext db = new ImageUploadDBContext();

        // GET: Image
        public ActionResult Index()
        {
            var lst = db.Images.ToList();
            System.Diagnostics.Debug.Print(lst.Count.ToString());
            return View(lst);
        }
        public ActionResult Create()
        {
            return View(new ImageViewModel());
        }

        // public ActionResult Create([Bind(Include = "ID, ImageTitle")] Image img, HttpPostedFileBase file)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID, ImageTitle, ImageUpload")] ImageViewModel img)
        {

            if (img.ImageUpload == null)
            {
                ModelState.AddModelError("ImageUpload", "This field is required.");
            }

            if (ModelState.IsValid)
            {
                var image = new Image();
                image.ImageTitle = img.ImageTitle;
                image.ImagePath = img.ImagePath;
                image.OriginalFileName = img.OriginalFileName;

                db.Images.Add(image);
                db.SaveChanges();
                // image.ID should hold the new id
                // create a folder structure like 000/000/000/000 to hold the uploaded
                // images (copy the generated images out of /uploads into /images/000/000/000/000
                // folder...
                // System.Diagnostics.Debug.Print(string.Format("New Id: {0}", image.ID));
                return RedirectToAction("Index");
            }

            return View(img);
        }

        public ActionResult Details(int id)
        {
            var image = db.Images.Find(id);
            if (image == null)
            {
                return new HttpNotFoundResult();
            }

            var model = new ImageViewModel
            {
                ImageTitle = image.ImageTitle,
                ImageDisplayUrl = image.ImagePath
            };

            return View(image);
        }

        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var image = db.Images.Find(id);
            if (image == null)
            {
                return new HttpNotFoundResult();
            }

            var model = new ImageViewModel
            {
                ImageTitle = image.ImageTitle,
                ImageDisplayUrl = image.ImagePath
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ImageViewModel model)
        {
            var validImageTypes = new string[]
            {
              "image/gif",
              "image/jpeg",
              "image/pjpeg",
              "image/png"
            };

            if (model.ImageUpload != null)
            {
                if (model.ImageUpload.ContentLength > 0)
                {

                    if (!validImageTypes.Contains(model.ImageUpload.ContentType))
                        ModelState.AddModelError("ImageUpload", "Please choose either a GIF, JPG or PNG image.");
                }
            }

            if (ModelState.IsValid)
            {
                var image = db.Images.Find(id);
                if (image == null)
                {
                    return new HttpNotFoundResult();
                }

                image.ImageTitle = model.ImageTitle;

                if (model.ImageUpload != null && model.ImageUpload.ContentLength > 0)
                {
                    // TODO: clear previous image before saving the new one...
                    var uploadDir = "~/images";
                    var imagePath = Path.Combine(Server.MapPath(uploadDir), model.ImageUpload.FileName);
                    var imageUrl = Path.Combine(uploadDir, model.ImageUpload.FileName);
                    model.ImageUpload.SaveAs(imagePath);
                    image.ImagePath = imageUrl;
                }

                db.Entry(image).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image img = db.Images.Find(id);
            if (img == null)
            {
                return HttpNotFound();
            }
            return View(img);
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Image img = db.Images.Find(id);
            if (img == null)
            {
                return HttpNotFound();
            }
            // remove the file from disk...
            try
            {
                string sFName = HttpContext.Server.MapPath(img.ImagePath);
                System.IO.File.Delete(sFName);
            }
            catch (Exception exc)
            {
                throw new HttpException(500, exc.Message);
            }
            db.Images.Remove(img);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}