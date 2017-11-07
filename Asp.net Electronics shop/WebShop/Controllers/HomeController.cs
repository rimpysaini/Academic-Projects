using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Faq()
        {
            ViewBag.Message = "Your faq page.";

            return View();
        }
        public ActionResult Sitemap()
        {
            DAO dao = new DAO();
            List<AdminCategory> l = dao.GetAllCategories();
            ViewBag.CategoryItems = l;

            return View();
        }
    }
}