using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class ProductController : Controller
    {
        DAO dao = new DAO();


        [HttpGet]
        public ActionResult Index()
        {

            List<AdminCategory> l = dao.GetAllCategories();
            ViewBag.CategoryItems = l;
            List<AdminProduct> pm = dao.GetAllProducts();
            ViewBag.ProductsList = pm;
            return View();
        }

        [HttpGet]
        public ActionResult AllProducts(string sortOrder)
        {
            List<AdminCategory> l = dao.GetAllCategories();
            ViewBag.CategoryItems = l;

            var products = from product in dao.GetAllProducts() select product;


            if (String.IsNullOrEmpty(sortOrder))
            {
                ViewBag.NameSortParm = "name_desc";
            }
            else
            {
                ViewBag.NameSortParm = "";
            }

            if (sortOrder == "price_desc")
            {
                ViewBag.PriceSortParm = "price_asc";
            }
            else
            {
                ViewBag.PriceSortParm = "price_desc";
            }

            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(s => s.Name);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(s => s.Price);
                    break;
                case "price_asc":
                    products = products.OrderBy(s => s.Price);
                    break;
                default:
                    products = products.OrderBy(s => s.Name);
                    break;
            }

            ViewBag.ProductsList = products.ToList();

            return View();
        }

        [HttpGet]
        public ActionResult Search(string search)
        {
            List<AdminCategory> l = dao.GetAllCategories();
            ViewBag.CategoryItems = l;
            var products = from product in dao.GetAllProducts() select product;
            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(product => product.Name.ToUpper().Contains(search.ToUpper()));
            }


            ViewBag.ProductsList = products.ToList();
            return View();
        }

        // Ivan
        public ActionResult Show(string id, string sortOrder)
        {
            DAO dao = new DAO();
            List<AdminCategory> l = dao.GetAllCategories();
            ViewBag.CategoryItems = l;

            var products = from product in dao.GetAllProductsByCatID(id) select product;


            if (String.IsNullOrEmpty(sortOrder))
            {
                ViewBag.NameSortParm = "name_desc";
            }
            else
            {
                ViewBag.NameSortParm = "";
            }

            if (sortOrder == "price_desc")
            {
                ViewBag.PriceSortParm = "price_asc";
            }
            else
            {
                ViewBag.PriceSortParm = "price_desc";
            }

            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(s => s.Name);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(s => s.Price);
                    break;
                case "price_asc":
                    products = products.OrderBy(s => s.Price);
                    break;
                default:
                    products = products.OrderBy(s => s.Name);
                    break;
            }

            ViewBag.ProductsList = products.ToList();

            return View();
        }

        public ActionResult Sale(string sortOrder)
        {
            List<AdminCategory> l = dao.GetAllCategories();
            ViewBag.CategoryItems = l;

            var products = from product in dao.GetAllProducts()
                           where product.Discount > 0
                           select product;

            if (String.IsNullOrEmpty(sortOrder))
            {
                ViewBag.NameSortParm = "name_desc";
            }
            else
            {
                ViewBag.NameSortParm = "";
            }

            if (sortOrder == "price_desc")
            {
                ViewBag.PriceSortParm = "price_asc";
            }
            else
            {
                ViewBag.PriceSortParm = "price_desc";
            }

            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(s => s.Name);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(s => s.Price);
                    break;
                case "price_asc":
                    products = products.OrderBy(s => s.Price);
                    break;
                default:
                    products = products.OrderBy(s => s.Name);
                    break;
            }


            ViewBag.ProductsList = products.ToList();

            return View();
        }

        public ActionResult ProductDetails(Product model)
        {
            List<DescriptionModel> pd = dao.GetSingleProductDetails(model.ID.ToString());
            ViewBag.DescriptionList = pd;
            List<int> quantityList = new List<int>() { 1, 2, 3, 4, 5 };
            ViewBag.Quantity = quantityList;
            return View(model);
        }


    }
}