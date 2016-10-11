using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using UserStore.WEB.Models;
using UserStore.BLL.DTO;
using System.Security.Claims;
using UserStore.BLL.Interfaces;
using UserStore.BLL.Infrastructure;
using UserStore.WEB.Convert;
using System.IO;
using System.Text;

namespace UserStore.WEB.Controllers
{
    public class HomeController : Controller
    {
        IContentService Service;
        public HomeController(IContentService service)
        {
            Service = service;
            pageSize = 12;
        }
        // GET: Home
        private int pageSize;
        private int PageSize
        {
            get
            {
                return pageSize;
            }
        }

        [HttpGet]
        [HandleError()]
        public ActionResult Search(string Search, string[] genres, string[] typ)
        {
            //var value = String.Join(";", genres.Concat(new string[] { Search }));
            string value = "";
            string Types = "";
            if (genres != null) foreach (var li in genres) value += ";" + li;
            if (Search != "") value += ";" + Search;
            if (typ != null) foreach (var li in typ) Types += ";" + li;
            return RedirectToAction("Index", "Home", new
            {
                types = Types != "" ? Types.Remove(0, 1) : "Book;Audio;Video;Empty",
                page = 1,
                filter = "search",
                value = value != "" ? value.ToLower().Remove(0, 1) : ""
            });

        }
        [Authorize]
        [HandleError()]
        public async Task<ActionResult> MakeVote(string user, int? vote, int id = 0)
        {
            OperationDetails details;


            details = await Service.MakeVote(id, user, vote);
            if (details.Succedeed)
                return PartialView("Content", ConvertTypeWEB.Convert(await Service.GetContent(id)));
            else return View("Error");

        }
        [Authorize(Roles = "admin")]
        [HandleError()]
        public async Task<ActionResult> MakeCheck(int id = 0, int check = 0)
        {
            OperationDetails details;
            details = await Service.CheckContent(id, check);
            if (details.Succedeed)
                return PartialView("Content", ConvertTypeWEB.Convert(await Service.GetContent(id)));
            else return View("Error");


        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [HandleError()]
        public async Task<ActionResult> DeleteContent(int id = 0, int check = 0)
        {
            OperationDetails details;
            details = await Service.CheckContent(id, check);
            if (details.Succedeed)
                return RedirectToAction("AdminIndex", new { types = "Book;Audio;Video;Empty", page = 1, filter = "admin", value = "" });
            else return View("Error");


        }
        [Authorize(Roles = "admin")]
        [HandleError()]
        public ActionResult UpdateContent(int id = 0)
        {

            return RedirectToAction("UpdateContent", "ContentOperation", new { id = id });


        }
        [HandleError()]
        public async Task<ActionResult> DisplayFullContent(int id, string types = "Book;Audio;Video;Empty", int page = 1, string filter = "home", string value = "")
        {

            ViewBag.Page = page;
            ViewBag.Filter = filter;
            ViewBag.Value = value;
            ViewBag.Types = types;

            List<ContentModelOutPut> list = new List<ContentModelOutPut>();
            list.Add(ConvertTypeWEB.Convert(await Service.GetContent(id)));
            PageModel pageModel = new PageModel { PageNumber = 1, PageSize = PageSize, TotalItems = 1 };
            IndexViewModel ivm = new IndexViewModel { PageInfo = pageModel, Contents = list, Genres = await Service.GetAllGenres() };
            return View("Index", ivm);

        }
        [HandleError()]
        public ActionResult GetVideo(string path)
        {

            string filePath = Path.Combine(Server.MapPath("~/AppContent"), path);
            if (Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                return File(filePath, "video/webm");
            }
            else return View("Error");
        }
        [HandleError()]
        public async Task<ActionResult> Index(string types = "Book;Audio;Video;Empty", int page = 1, string filter = "home", string value = "")
        {
            ViewBag.Page = page;
            ViewBag.Filter = filter;
            ViewBag.Value = value;
            ViewBag.Types = types;
            if (filter != "admin")
            {
                List<ContentModelOutPut> list = new List<ContentModelOutPut>();
                foreach (var li in await Service.GetContent(page, PageSize, filter, value, types))
                    list.Add(ConvertTypeWEB.Convert(li));
                PageModel pageModel = new PageModel { PageNumber = page, PageSize = PageSize, TotalItems = await Service.GetContentCount(filter, value, types) };
                IndexViewModel ivm = new IndexViewModel { PageInfo = pageModel, Contents = list, Genres = await Service.GetAllGenres(), Filter = filter, Value = value, Types = types };
                return View(ivm);
            }
            else
            {
                return RedirectToAction("AdminIndex", new { types = types, page = page, filter = filter, value = value });
            }

        }
        [Authorize(Roles = "admin")]
        [HandleError()]
        public async Task<ActionResult> AdminIndex(string types = "Book;Audio;Video;Empty", int page = 1, string filter = "admin", string value = "")
        {



            List<ContentModelOutPut> list = new List<ContentModelOutPut>();
            foreach (var li in await Service.GetContent(page, PageSize, filter, value, types)) list.Add(ConvertTypeWEB.Convert(li));
            PageModel pageModel = new PageModel { PageNumber = page, PageSize = PageSize, TotalItems = await Service.GetContentCount(filter, value, types) };
            IndexViewModel ivm = new IndexViewModel { PageInfo = pageModel, Contents = list, Genres = await Service.GetAllGenres(), Filter = filter, Value = value, Types = types };
            ViewBag.Page = page;
            ViewBag.Filter = filter;
            ViewBag.Value = value;
            ViewBag.Types = types;
            return View("Index", ivm);

        }
        [Authorize]
        [HandleError()]
        public ActionResult Download(string name, string filePath)
        {

            string path = Path.Combine(Server.MapPath("~/AppContent"), filePath);
            if (Directory.Exists(Path.GetDirectoryName(path)))
            {
                return File(path, System.Net.Mime.MediaTypeNames.Application.Octet, name + Path.GetExtension(path));
            }
            else return View("Error");


        }
        [Authorize(Roles = "admin")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
    }
}