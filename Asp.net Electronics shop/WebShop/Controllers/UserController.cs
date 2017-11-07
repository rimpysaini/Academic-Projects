using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class UserController : Controller
    {
        DAO dao = new DAO();
        // GET: User
        public ActionResult Index()
        {
            return View("Register");
        }


        [HttpGet]
        public ActionResult Register()
        {
            return View("Register");
        }


        [HttpPost]
        public ActionResult Register(UserModel user)
        {
            int count = 0;
            if (ModelState.IsValid)
            {
                count = dao.Insert(user, DateTime.Now);
                //Response.Write(dao.message);
                if (count > 0)
                {
                    return View("Login");
                }
                else
                {
                    ViewBag.Status = "Error!" + dao.message;
                    return View("Status");
                }                
            }
            return View(user);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();

        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();

        }


        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult ResetPassWord(UserModel user)
        {
            int count = 0;
            ModelState.Remove("FirstName");
            ModelState.Remove("LastName");
            ModelState.Remove("Email");
            ModelState.Remove("Phone");
            ModelState.Remove("SecurityQuestion");
            ModelState.Remove("Answer");
            ModelState.Remove("County");
            ModelState.Remove("Town");
            ModelState.Remove("Street1");
            user.Email = Session["email"].ToString();


            if (ModelState.IsValid)
            {
                count = dao.ResetPassword(user);
                if (count > 0)
                {
                    ViewBag.Status = "Password successfully reset";

                    return View("Status");
                }
                else
                {
                    ViewBag.Status = "Error! " + dao.message;
                    return View("Status");
                }

            }
            else return View();

        }


        [HttpPost]
        public ActionResult ForgotPassword(FormCollection form)
        {

            string email = form["email"];
            string question = null;
            question = dao.GetSecurityQuestion(email);

            if (question != null)
            {

                TempData["email"] = email;
                ViewBag.Question = question;
                return View("SecurityQuestion");

            }
            else
            {
                ViewBag.Status = "Error, you did not provide us with a valid e-mail address";
                return View("Status");
            }



        }


        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult SecurityQuestion(UserModel user)
        {
            ModelState.Remove("FirstName");
            ModelState.Remove("LastName");
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            ModelState.Remove("Phone");
            ModelState.Remove("SecurityQuestion");
            ModelState.Remove("Email");
            ModelState.Remove("County");
            ModelState.Remove("Town");
            ModelState.Remove("Street1");

            if (TempData["email"] != null)
            {
                user.Email = TempData["email"].ToString();
                if (ModelState.IsValid)
                {
                    user.FirstName = dao.CheckSecurityQuestion(user);
                    if (user.FirstName != null)
                    {
                        Session["name"] = user.FirstName;
                        Session["email"] = user.Email;

                        return RedirectToAction("ResetPassword", "User");
                    }
                    else
                    {
                        ViewBag.Status = "Error! " + dao.message;
                        return View("Status");
                    }

                }
                else return View();

            }

            else return View("ForgotPassWord");

        }


        [HttpPost]
        public ActionResult Login(UserModel user)
        {
            ModelState.Remove("FirstName");
            ModelState.Remove("LastName");
            ModelState.Remove("ConfirmPassword");
            ModelState.Remove("Phone");
            ModelState.Remove("SecurityQuestion");
            ModelState.Remove("Answer");
            ModelState.Remove("County");
            ModelState.Remove("Town");
            ModelState.Remove("Street1");
            string sEmail = user.Email;
            string sPass = user.Password;

            if (ModelState.IsValid)
            {
                user.FirstName = dao.CheckLogin(user);
                if (user.FirstName != null)
                {
                    Session["name"] = user.FirstName;
                    Session["email"] = user.Email;

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (DisplaySomeInfo(sEmail, sPass))
                        return RedirectToAction("Index", "Admin");
                    else
                    {
                        ViewBag.Status = "Invalid login details!";
                        return View();
                    }
                }
            }
            else return View(user);
        }

        private bool DisplaySomeInfo(string e, string p)
        {
            // Check => maybe is admin account
            int role = dao.CheckLoginAdmin(e, p);
            if (role == 1)
            {
                Session["important"] = "admin";
                return true;
            }
            else if (role == 2)
            {
                Session["juststaff"] = "staff";
                return true;
            }
            else return false;
            // End Admin login
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            return View("../Home/Index");
        }

    }
}