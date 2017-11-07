using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class SupplierController : Controller
    {
        DAO dao = new DAO();

        [HttpGet]
        public ActionResult Index()
        {
            List<SupplierModel> s = dao.GetAllSuppliers();
            ViewBag.SupplierItems = s;
            return View();
        }

        [HttpGet]
        public ActionResult AddSupplier()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSupplier(SupplierModel supplier)
        {
            int count = 0;
            if (ModelState.IsValid)
            {
                count = dao.InsertSupplier(supplier);
                if (count > 0)
                {
                    return RedirectToAction("Index", "Supplier");
                }
                else
                {
                    ViewBag.Message = "Error! " + dao.message;
                }
                ModelState.Clear();
                return View();
            }
            else return View("AddSupplier", supplier);
        }

        [HttpGet]
        public ActionResult UpdateSupplier(string id)
        {
            List<SupplierModel> item = dao.GetSingleSupplier(id);
            foreach (var s in item)
            {
                ViewBag.mid = s.Id.ToString();
                ViewBag.name = s.Name.ToString();
                ViewBag.address = s.Address.ToString();
                ViewBag.phone = s.Phone.ToString();
                ViewBag.email = s.Email.ToString();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSupplier(SupplierModel supplier,string id)
        {
            int count = 0;
            if (ModelState.IsValid)
            {
                count = dao.UpdateSupplier(supplier,id);
                if (count > 0)
                {
                    return RedirectToAction("Index", "Supplier");
                }
                else
                {
                    ViewBag.Message = "Error! " + dao.message;
                }
                ModelState.Clear();
                return View();
            }
            return RedirectToAction("Index", "Supplier");
        }
    }
}