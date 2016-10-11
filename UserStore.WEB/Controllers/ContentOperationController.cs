using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

using System.Threading.Tasks;
using UserStore.WEB.Models;

using System.IO;

using UserStore.BLL.Interfaces;
using UserStore.BLL.Infrastructure;
using System.Linq;
using UserStore.WEB.Convert;
using UserStore.WEB.Infrastructure;

namespace UserStore.WEB.Controllers
{
    public class ContentOperationController : Controller
    {
        // GET: Content
        IContentService Service;
        public ContentOperationController(IContentService service)
        {
            Service = service;
        }
        private ContentModelInPut Model
        {
            get; set;
        }
        [Authorize]
        public ActionResult StartNewContent()
        {
            Model = new ContentModelInPut();
            Model.operation = "AddNewContent";
            return View("NewContent", Model);
        }



        private async Task<List<string>> GetGenresFromDB()
        {
            try
            {


                return await Service.GetAllGenres();
            }
            catch
            {
                return null;
            }
        }


        private string Save(HttpPostedFileBase file)
        {
            try
            {
                if (file != null && file.ContentType != null)
                {

                    string type = file.ContentType.ToLower().Split(new char[] { '/' }).FirstOrDefault();
                    if (type != null && (type == "video" || type == "audio" || type == "image" || type == "application"))
                    {
                        if (file.ContentLength > 0)
                        {
                            string pathToFolderOfContents = "~/AppContent";
                            var fileName = Path.GetFileName(file.FileName);
                            string nameOdDirectoryForContent = DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace(".", "");
                            string fullPathToContent = Path.Combine(Server.MapPath(pathToFolderOfContents), nameOdDirectoryForContent);
                            if (!Directory.Exists(fullPathToContent)) Directory.CreateDirectory(fullPathToContent);
                            string path4 = Path.Combine(fullPathToContent, fileName);
                            file.SaveAs(path4);

                            return nameOdDirectoryForContent + @"/" + fileName;
                        }
                        return null;

                    }
                    return null;
                }
                return "";
            }
            catch(Exception ex)
            {
                throw new SaveContentException("Saving is failed", ex);
            }


        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError()]
        public async Task<ActionResult> AddNewContent(ContentModelInPut Model)
        {

            if (ModelState.IsValid)
            {
                string pathImage = Save(Model.Image);
                string pathContent = Save(Model.Path);
                if (pathContent != null && pathImage != null)
                {
                    OperationDetails det = await Service.CreateContent(ConvertTypeWEB.Convert(Model, pathImage, pathContent));
                    if (det.Succedeed)
                        return Redirect("/Home/Index");
                    else return View("Error");
                }
                ModelState.AddModelError("Enter ", "File has wrong type");
                return View("NewContent", Model);
            }
            else
            {
                ModelState.AddModelError("Enter ", "Field(s) is/are empty");
                return View("NewContent", Model);
            }

        }
        [Authorize(Roles = "admin")]
        [HandleError()]
        public async Task<ActionResult> UpdateContent(int id)
        {
            ContentModelInPut model;
            model = ConvertTypeWEB.ConvertIn(await Service.GetContent(id));
            return View("NewContent", model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError()]
        public async Task<ActionResult> Update(ContentModelInPut Model)
        {

            if (ModelState.IsValid)
            {
                string pathImage = Save(Model.Image);
                if (pathImage == null) pathImage = "";
                OperationDetails det = await Service.UpdateContent(ConvertTypeWEB.Convert(Model, pathImage, ""));
                if (det.Succedeed)
                    return Redirect("/Home/Index");
                else return View("Error");
            }
            else
            {
                ModelState.AddModelError("Enter ", "Field(s) is/are empty");
                return View("NewContent", Model);
            }


        }
    }
}