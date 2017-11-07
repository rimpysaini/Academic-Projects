using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class AdminProductController : Controller
    {


        [HttpGet]
        public ActionResult Index()
        {
            if (Session["important"] != null || Session["juststaff"] != null)
            {

                DAO dao = new DAO();
                List<AdminProduct> pm = dao.GetAllProducts();
                ViewBag.ProductsList = pm;
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public ActionResult AddProduct()
        {
            if (Session["important"] != null || Session["juststaff"] != null)
            {


                DAO dao = new DAO();
                // Build categories dropdown
                Dictionary<int, string> catDict = dao.GetAllCategoryNames();
                SelectList catList = new SelectList(
                catDict.Select(x => new { Value = x.Key, Text = x.Value }),
                "Value",
                "Text"
                );
                ViewBag.Categories = catList;
                // Build supplier dropdown
                Dictionary<int, string> supDict = dao.GetAllSupplierNames();
                SelectList supList = new SelectList(
                supDict.Select(x => new { Value = x.Key, Text = x.Value }),
                "Value",
                "Text"
                );
                ViewBag.Suppliers = supList;

                return View();
            }
            else return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProduct(AdminProductModel product)
        {
            if (Session["important"] != null || Session["juststaff"] != null)
            {


                DAO dao = new DAO();
                DescriptionModel desc = new DescriptionModel();
                List<DescriptionModel> descList = new List<DescriptionModel>();
                descList.Add(desc);
                product.Details = descList;
                /**
                 *  1. Upload fileto images dir and generate file name
                 *  2. Create new category with that imgUrl file name
                 */
                string defaultImgUrlPath = Server.MapPath("~/Content/Images/no_image.png");
                string newImgUrlPath = "";
                string fileName = "";
                int count = 0;
                string ext;
                string extractedName;
                var validImageTypes = new string[]
                    {
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png"
                    };
                // Upload the image
                if (product.Image != null && product.Image.ContentLength > 0)
                    if (!validImageTypes.Contains(product.Image.ContentType))
                    {
                        ViewBag.Message = "ERROR: Unknown image type";
                    }
                    else
                    {
                        // Create new category
                        if (ModelState.IsValid)
                        {
                            try
                            {
                                fileName = Path.GetFileName(product.Image.FileName);
                                // Change file name
                                // Extract name/extension
                                extractedName = fileName.Split('.')[0];
                                ext = fileName.Split('.')[1];
                                // Create new name+ext
                                fileName = product.Name.ToLower() + "." + ext;
                                // Save to DB
                                count = dao.InsertProduct(product, fileName);
                            }
                            catch (Exception ex)
                            {
                                ViewBag.Status = "DB ERROR!" + ex.Message;
                            }
                            if (count == 1)
                            {
                                // Upload the file
                                newImgUrlPath = Path.Combine(Server.MapPath("~/Content/Images/products/"), fileName);
                                product.Image.SaveAs(newImgUrlPath);
                                ModelState.Clear();
                                return RedirectToAction("Index", "AdminProduct");
                            }
                            else
                            {
                                ViewBag.Status = "ERROR! Product create fail";
                            }
                            return View(); // Display modal to say category created
                        }
                        else
                        {
                            ViewBag.Status = "ERROR! Model Invalid";
                        }
                    }
                else
                {
                    // No image file uploaded
                    ViewBag.Status = "ERROR: No image selected";
                }
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult UpdateDesc(string id)
        {
            if (Session["important"] != null || Session["juststaff"] != null)
            {
                DAO dao = new DAO();
                List<DescriptionModel> pd = dao.GetSingleProductDetails(id);
                ViewBag.DescriptionList = pd;
                ViewBag.ID = id;
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult UpdateDesc(DescriptionModel dm, string id)
        {
            if (Session["important"] != null || Session["juststaff"] != null)
            {
                DAO dao = new DAO();
                List<DescriptionModel> pd = dao.GetSingleProductDetails(id);
                ViewBag.DescriptionList = pd;
                ViewBag.ID = id;


                // Update XML file
                int count = 0;
                if (ModelState.IsValid)
                {
                    try
                    {
                        count = dao.UpdateProductDescription(dm, id);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR: " + ex.Message;
                    }
                    if (count > 0)
                    {
                        return RedirectToAction("UpdateDesc", "AdminProduct", new { @id = id });
                    }
                    else
                    {
                        ViewBag.Message = "ERROR: Description update fail!";
                    }
                }
                else
                {
                    ViewBag.Message = "ERROR: Invalid model!";
                }
                ViewBag.ID = id;
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult UpdateProduct(string id)
        {
            if (Session["important"] != null || Session["juststaff"] != null)
            {

                DAO dao = new DAO();
                int catNameValue = 0;
                int supNameValue = 0;

                List<AdminProduct> item = dao.GetSingleProduct(id);
                foreach (var s in item)
                {
                    ViewBag.mid = s.Id.ToString();
                    ViewBag.name = s.Name.ToString();
                    ViewBag.price = s.Price.ToString();
                    ViewBag.discount = s.Discount.ToString();
                    ViewBag.url = s.ImageUrl.ToString();
                    TempData["oldImage"] = s.ImageUrl.ToString();
                    catNameValue = s.CategoryID;
                    supNameValue = s.SupplierID;
                }
                // Extract values categoryID,suplierID from dictionary
                // Set selected key/value

                // Build categories dropdown
                Dictionary<int, string> catDict = dao.GetAllCategoryNames();
                SelectList catList = new SelectList(catDict.Select(x => new { Value = x.Key, Text = x.Value }), "Value", "Text");
                // Build supplier dropdown
                Dictionary<int, string> supDict = dao.GetAllSupplierNames();
                SelectList supList = new SelectList(supDict.Select(x => new { Value = x.Key, Text = x.Value }), "Value", "Text");

                ViewBag.Suppliers = supList;
                ViewBag.Categories = catList;
                ViewBag.supID = supNameValue;
                ViewBag.catID = catNameValue;

                return View();
            }
            else return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProduct(AdminProductModel product, string id)
        {
            if (Session["important"] != null || Session["juststaff"] != null)
            {


                DAO dao = new DAO();
                /**
                 *  1. Upload fileto images dir and generate file name
                 *  2. Create new category with that imgUrl file name
                 */
                string defaultImgUrlPath = Server.MapPath("~/Content/Images/no_image.png");
                string newImgUrlPath = "";
                string fileName = "";
                int count = 0;
                string ext;
                string extractedName;
                var validImageTypes = new string[]
                    {
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png"
                    };
                // Upload the image
                if (product.Image != null && product.Image.ContentLength > 0)
                    if (!validImageTypes.Contains(product.Image.ContentType))
                    {
                        ViewBag.Status = "ERROR: Unknown image type";
                    }
                    else
                    {
                        // Create new category
                        if (ModelState.IsValid)
                        {
                            try
                            {
                                fileName = Path.GetFileName(product.Image.FileName);
                                // Change file name
                                // Extract name/extension
                                extractedName = fileName.Split('.')[0];
                                ext = fileName.Split('.')[1];
                                // Create new name+ext
                                fileName = product.Name.ToLower() + "." + ext;
                                // Update product in DB
                                count = dao.UpdateProduct(product, fileName, id);

                            }
                            catch (Exception ex)
                            {
                                ViewBag.Status = "DB ERROR!" + ex.Message;
                            }
                            if (count == 1)
                            {
                                ModelState.Clear();
                                // Get full path 
                                newImgUrlPath = Path.Combine(Server.MapPath("~/Content/Images/products"), fileName);
                                // Delete old image
                                string fullPath = Server.MapPath("~/Content/Images/products/" + TempData["oldImage"].ToString());
                                if (System.IO.File.Exists(fullPath))
                                {
                                    System.IO.File.Delete(fullPath);
                                }
                                // Upload the file
                                product.Image.SaveAs(newImgUrlPath);
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                ViewBag.Status = "ERROR! Product create fail";
                            }
                            return View(); // Display modal to say category created
                        }
                        else
                        {
                            ViewBag.Status = "ERROR! Model Invalid";
                        }
                    }
                else
                {
                    // No image file uploaded
                    ViewBag.Status = "ERROR: No image selected";
                }
                return View();
            }
            else return RedirectToAction("Index", "Home");

        }

    }
}