using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        DAO dao = new DAO();
        static List<Product> selectedProducts = new List<Product>();
        static List<ItemModel> selectedItems = new List<ItemModel>();

        decimal totalPrice = 0.0m;
        // GET: Cart
        public ActionResult Index()
        {
            return RedirectToAction("ViewCart");
        }

        public ActionResult ViewCart()
        {

            if (Session["email"] != null)
            {
                string email = Session["email"].ToString();

                List<Product> list = dao.ShowAllProductsInCart(email);

                foreach (Product product in list)
                {
                    product.TotalPrice = product.Quantity * product.Price;
                    totalPrice = totalPrice + product.TotalPrice;


                }
                dao.UpdateCartCost(email, totalPrice);
                ViewBag.TransactionPrice = totalPrice;

                return View(list);
            }

            else return RedirectToAction("Login", "User");
        }

        [HttpPost]
        public ActionResult AddProduct(FormCollection form)
        {
            if (Session["email"] != null)
            {

                int id = int.Parse(Request.Form["productID"]);
                int quantity = int.Parse(Request.Form["quantity"]);

                // get price of a product
                string email = Session["email"].ToString();
                List<Product> list = dao.ShowAllProductsInCart(email);
                foreach (Product product in list)
                {
                    product.TotalPrice = product.Quantity * product.Price;
                    totalPrice = totalPrice + product.TotalPrice;
                }
                if (Request.IsAjaxRequest())
                {
                    //ViewBag.Cart_total = totalPrice;
                    return Content(totalPrice.ToString());
                    //return PartialView("Cart_total");
                }




                //string email = Session["email"].ToString();
                bool exit = false;
                int newquantity = 0;
                //int id = int.Parse(form["productID"]);
                //int quantity = int.Parse(form["quantity"]);
                //List<Product> list = dao.ShowAllProductsInCart(email);
                if (list.Count == 0)
                {
                    dao.InsertProductIntoCart(email, id, quantity);
                }
                else
                {

                    do foreach (Product product in list) //checking to see if it's already in the selected items
                        {
                            if (product.ID == id)
                            {
                                newquantity = product.Quantity + quantity;
                                dao.ChangeProductQuantity(email, id, newquantity);
                                exit = true;
                            }
                            else
                            {
                                dao.InsertProductIntoCart(email, id, quantity);
                                exit = true;
                            }
                        } while (!exit);
                }

                return RedirectToAction("ViewCart");
            }

            else return RedirectToAction("Login", "User");

        }
        [HttpPost]
        public ActionResult RemoveItem(FormCollection form)
        {
            string email = Session["email"].ToString();
            int productid = int.Parse(form["productID"]);
            dao.RemoveProductFromCart(email, productid);

            return RedirectToAction("ViewCart");
        }

        [HttpPost]
        public ActionResult ChangeQuantity(FormCollection form)
        {

            string email = Session["email"].ToString();
            int productid = int.Parse(form["productID"]);
            int quantity = int.Parse(form["quantity"]);
            int newquantity = int.Parse(form["newquantity"]);
            newquantity = quantity + newquantity;
            if (newquantity <= 0)
            {
                dao.RemoveProductFromCart(email, productid);
            }
            else
            {
                dao.ChangeProductQuantity(email, productid, newquantity);
            }


            return RedirectToAction("ViewCart");
        }


        public ActionResult ClearAll(FormCollection form)
        {
            string email = Session["email"].ToString();
            dao.RemoveAllProductsFromCart(email);
            return RedirectToAction("ViewCart");
        }

        [HttpGet]
        public ActionResult Checkout()
        {
            List<int> shippinglist = new List<int>() { 1, 2, 3 };
            ViewBag.Shipping = shippinglist;
            string email = Session["email"].ToString();
            List<Product> list = dao.ShowAllProductsInCart(email);
            if (list.Count > 0)
            {
                foreach (var product in list)
                {
                    product.TotalPrice = product.Quantity * product.Price;
                    totalPrice = totalPrice + product.TotalPrice;


                }
            }
            decimal.Round(totalPrice, 2);
            ViewBag.Price = totalPrice;
            return View(list);
        }

        [HttpPost]
        public ActionResult CheckOut(FormCollection form)
        {

            int count = 0;
            int count1 = 0;
            int count2 = 0;

            string email = Session["email"].ToString();
            int shippingid = int.Parse(form["shipping"]);
            string creditcardnumber = form["creditcard"];

            decimal shippingcharges = dao.CheckShippingCharges(shippingid);
            dao.AddShippingToCart(shippingid, email);

            List<Product> list = dao.ShowAllProductsInCart(email);
            if (list.Count > 0)
            {
                foreach (var product in list)
                {
                    product.TotalPrice = product.Quantity * product.Price;
                    totalPrice = totalPrice + product.TotalPrice;

                }
            }


            totalPrice = totalPrice + shippingcharges;
            decimal.Round(totalPrice, 2);
            ViewBag.Price = totalPrice;
            dao.UpdateCartCost(email, totalPrice);

            if (Businesslogic.Mod10Check(creditcardnumber) == true)
            {


                count = dao.AddTransaction(Session.SessionID + count, email, DateTime.Now, totalPrice);
                if (count > 0) //if the transaction is successfully placed, then the cart status is changed and a new cart is created for this customer
                {

                    count1 = dao.UpdateCartAfterSuccessfulTransaction(email, DateTime.Now);
                    if (count1 > 0)
                    {

                        count2 = dao.CreateCartAfterSuccessfulTransaction(email);
                        if (count2 > 0)
                        {
                            ViewBag.Status = "Order succesfully placed, we'll be in touch!";
                        }
                        else
                        {
                            ViewBag.Status = "Error!" + dao.message;
                        }
                    }
                    else
                    {
                        ViewBag.Status = "Error!" + dao.message;
                    }

                }
            }
            else
            {
                ViewBag.Status = "You have not entered a valid credit card number";
            }

            return View("Status");
        }

        public ActionResult UpdateShipping(string id)
        {
            string email = Session["email"].ToString();
            int shippingid = int.Parse(id);
           

            decimal shippingcharges = dao.CheckShippingCharges(shippingid);
            dao.AddShippingToCart(shippingid, email);

            List<Product> list = dao.ShowAllProductsInCart(email);
            if (list.Count > 0)
            {
                foreach (var product in list)
                {
                    product.TotalPrice = product.Quantity * product.Price;
                    totalPrice = totalPrice + product.TotalPrice;

                }
            }

            totalPrice = totalPrice + shippingcharges;
            decimal.Round(totalPrice, 2);
            ViewBag.Price = totalPrice;

            return View("Checkout", list);
        }


    }


}