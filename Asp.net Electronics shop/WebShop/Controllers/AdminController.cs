using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class AdminController : Controller
    {

        DAO dao = new DAO();

        [HttpGet]
        public ActionResult Index()
        {
            if (Session["important"] != null || Session["juststaff"] != null)
            {
                return View();

            }
            else return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["important"] != null || Session["juststaff"] != null)
            {
                return View();

            }
            else return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Login(StaffLoginModel lsm)
        {
            if (Session["important"] != null || Session["juststaff"] != null)
            {
                int count = 0;
                if (ModelState.IsValid)
                {
                    count = dao.LoginStaff(lsm);
                    if (count > 0)
                    {
                        return View();
                    }
                    else
                    {
                        ViewBag.Message = "Error! " + dao.message;
                    }
                    ModelState.Clear();
                    return View();
                }
                else return View("Login");
            }
            else return RedirectToAction("Index", "Home");
        }
    }
}