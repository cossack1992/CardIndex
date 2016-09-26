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
            get;set;
        }
        [Authorize]       
        public  ActionResult StartNewContent()
        {
            Model = new ContentModelInPut();
            Model.operation = "AddNewContent";
            return  View("NewContent", Model);
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
                if (file != null)
                {
                    if (file.ContentLength > 0)
                    {
                        string path = "~/AppContent";
                        var str1 = Path.GetFileName(file.FileName);
                        string path2 = DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace(".", "");
                        string path3 = Path.Combine(Server.MapPath(path), path2);
                        if (!Directory.Exists(path3)) Directory.CreateDirectory(path3);
                        string path4 = Path.Combine(path3, str1);
                        file.SaveAs(path4);
                        return path2 + @"/" + str1;
                    }
                }
                return "";
            }
            catch
            {
                return "";
            }    
                
            
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError(View = "Error")]
        public async Task<ActionResult> AddNewContent(ContentModelInPut Model)
        {
            
                if (ModelState.IsValid)
                {
                    
                    OperationDetails det = await Service.CreateContent(ConvertTypeWEB.Convert(Model, Save(Model.Image), Save(Model.Path)));
                    if(det.Succedeed)
                    return Redirect("/Home/Index");
                    else return View("Error");
                }
                else
                {
                    ModelState.AddModelError("Enter ", "Field(s) is/are empty");
                    return View("NewContent", Model);
                }
            
        }
        [Authorize(Roles = "admin")]
        [HandleError(View = "Error")]
        public async Task<ActionResult> UpdateContent(int id)
        {
            ContentModelInPut model;
            model = ConvertTypeWEB.ConvertIn(await Service.GetContent(id));
            return View("NewContent", model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError(View = "Error")]
        public async Task<ActionResult> Update(ContentModelInPut Model)
        {
            
                if (ModelState.IsValid)
                {
                    OperationDetails det = await Service.UpdateContent(ConvertTypeWEB.Convert(Model, Save(Model.Image), Save(Model.Path)));
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