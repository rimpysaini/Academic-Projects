using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class StaffController : Controller
    {
        DAO dao = new DAO();

        [HttpGet]
        public ActionResult Index()
        {
            if (Session["important"] != null || Session["juststaff"] != null)
            {
                List<StaffModel> s = dao.GetAllStaff();
                ViewBag.StaffItems = s;
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult AddStaff()
        {
            if (Session["important"] != null || Session["juststaff"] != null)
            {
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStaff(StaffModel staff)
        {
            if (Session["important"] != null || Session["juststaff"] != null)
            {

                int count = 0;
                if (ModelState.IsValid)
                {
                    count = dao.InsertStaff(staff);
                    if (count > 0)
                    {
                        return RedirectToAction("Index", "Staff");
                    }
                    else
                    {
                        ViewBag.Message = "Error! " + dao.message;
                    }
                    ModelState.Clear();
                    return View();
                }
                else return View("AddStaff", staff);
            }
            else return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        public ActionResult UpdateStaff(string id)
        {
            if (Session["important"] != null || Session["juststaff"] != null)
            {

                List<UpdateStaffModel> item = dao.GetSingleStaff(id);
                foreach (var s in item)
                {
                    ViewBag.mid = s.Id.ToString();
                    ViewBag.fname = s.FirstName.ToString();
                    ViewBag.lname = s.LastName.ToString();
                    ViewBag.role = s.UserRole.ToString();
                    ViewBag.state = s.UserState.ToString();
                    ViewBag.email = s.Email.ToString();
                }
                UpdateStaffModel model = new UpdateStaffModel();
                try
                {
                    model.UserRole = (Role)Enum.Parse(typeof(Role), ViewBag.role);
                    model.UserState = (Active)Enum.Parse(typeof(Active), ViewBag.state);
                }
                catch (Exception)
                {
                    return RedirectToAction("Index", "Staff");
                }
                return View(model);
            }
            else return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStaff(UpdateStaffModel staff, string id)
        {
            if (Session["important"] != null || Session["juststaff"] != null)
            {

                int count = 0;
                if (ModelState.IsValid)
                {
                    count = dao.UpdateSingleStaff(staff, id);
                    if (count > 0)
                    {
                        return RedirectToAction("Index", "Staff");
                    }
                    else
                    {
                        ViewBag.mid = id;
                        ViewBag.Message = "Error! " + dao.message;
                    }
                    ModelState.Clear();
                    return View();
                }
                else
                {
                    ViewBag.mid = id;
                    return View();
                }
            }
            else return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult ResetPass(string id)
        {
            if (Session["important"] != null || Session["juststaff"] != null)
            {
                ViewBag.mid = id;
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult ResetPass(PasswordResetModel pr, string id)
        {
            if (Session["important"] != null || Session["juststaff"] != null)
            {
                int count = 0;
                if (ModelState.IsValid)
                {
                    count = dao.UpdateStaffPassword(pr, id);
                    if (count > 0)
                    {
                        return RedirectToAction("Index", "Staff");
                    }
                    else
                    {
                        ViewBag.mid = id;
                        ViewBag.Message = "Error! " + dao.message;
                    }
                    ModelState.Clear();
                    return View();
                }
                else
                {
                    ViewBag.mid = id;
                    return View();
                }
            }
            else return RedirectToAction("Index", "Home");
        }
    }
}