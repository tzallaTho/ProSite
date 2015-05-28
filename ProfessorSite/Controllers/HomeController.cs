using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProfessorSite.Models;

namespace ProfessorSite.Controllers
{
    public class HomeController : Controller
    {
        // GET: /Home/

        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }


        public ActionResult GetSearchIProfs()
        {
            return View(new MyViewModel
            {
                IsDyslexia = true
            });
        }

        [HttpPost]
        public ActionResult GetSearchIProfs(MyViewModel model)
        {
            return Content("IsFemale: " + model.IsDyslexia);
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Φόρμα Επικοινωνίας";
            return View();
        }
    }
}
