using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserStore.WEB.Controllers
{
    public class ContentController : Controller
    {
        // GET: Content
        public ActionResult NewContent()
        {
            return View();
        }
    }
}