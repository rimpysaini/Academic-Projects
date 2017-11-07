using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class AdminCategoryController : Controller
    {
        DAO dao = new DAO();

        [HttpGet]
        public ActionResult Index()
        {
            if (Session["important"] != null || Session["juststaff"] != null)
            {
                List<AdminCategory> l = dao.GetAllCategories();
                ViewBag.CategoryItems = l;
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult AddCategory()
        {
            if (Session["important"] != null || Session["juststaff"] != null)
            {

                return View();
            }
            else return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCategory(AdminCategoryModel categoryModel)
        {
            if (Session["important"] != null || Session["juststaff"] != null)
            {

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
                if (categoryModel.Image != null && categoryModel.Image.ContentLength > 0)
                    if (!validImageTypes.Contains(categoryModel.Image.ContentType))
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
                                fileName = Path.GetFileName(categoryModel.Image.FileName);
                                // Change file name
                                // Extract name/extension
                                extractedName = fileName.Split('.')[0];
                                ext = fileName.Split('.')[1];
                                // Create new name+ext
                                fileName = categoryModel.Name.ToLower() + "." + ext;
                                // Save to DB
                                count = dao.InsertCategory(categoryModel.Name, categoryModel.Description, fileName);
                            }
                            catch (Exception ex)
                            {
                                ViewBag.Status = "DB ERROR!" + ex.Message;
                            }
                            if (count == 1)
                            {
                                // Upload the file
                                newImgUrlPath = Path.Combine(Server.MapPath("~/Content/Images/category/"), fileName);
                                categoryModel.Image.SaveAs(newImgUrlPath);
                                ModelState.Clear();
                                return RedirectToAction("Index", "AdminCategory");
                            }
                            else
                            {
                                ViewBag.Status = "ERROR! Category create fail";
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
        public ActionResult UpdateCategory(string id)
        {
            if (Session["important"] != null || Session["juststaff"] != null)
            {
                List<AdminCategory> item = dao.GetSingleCategory(id);
                foreach (var s in item)
                {
                    ViewBag.mid = s.Id.ToString();
                    ViewBag.name = s.Name.ToString();
                    ViewBag.desc = s.Description.ToString();
                    ViewBag.url = s.ImageUrl.ToString();
                    TempData["oldImage"] = s.ImageUrl.ToString();
                }
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCategory(AdminCategoryModel categoryModel, string id)
        {
            if (Session["important"] != null || Session["juststaff"] != null)
            {
                /**
                 *  1. Upload fileto images dir and generate file name
                 *  2. Create new category with that imgUrl file name
                 */
                //string defaultImgUrlPath = Server.MapPath("~/Content/Images/no_image.png");
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

                if (categoryModel.Image != null && categoryModel.Image.ContentLength > 0)
                {
                    if (!validImageTypes.Contains(categoryModel.Image.ContentType))
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
                                fileName = Path.GetFileName(categoryModel.Image.FileName);
                                // Change file name
                                // Extract name/extension
                                extractedName = fileName.Split('.')[0];
                                ext = fileName.Split('.')[1];
                                // Create new name+ext
                                fileName = categoryModel.Name.ToLower() + "." + ext;
                                // Save to DB
                                count = dao.UpdateCategory(id, categoryModel.Name, categoryModel.Description, fileName);
                                if (count == 1)
                                {
                                    ModelState.Clear();
                                    // Get full path 
                                    newImgUrlPath = Path.Combine(Server.MapPath("~/Content/Images/category"), fileName);
                                    // Delete old image
                                    string fullPath = Server.MapPath("~/Content/Images/category/" + TempData["oldImage"].ToString());
                                    if (System.IO.File.Exists(fullPath))
                                    {
                                        System.IO.File.Delete(fullPath);
                                    }
                                    // Upload the file
                                    categoryModel.Image.SaveAs(newImgUrlPath);
                                    return RedirectToAction("Index");
                                }
                                else
                                {
                                    ViewBag.Status = "ERROR! Category update fail, duplicate category!";
                                }
                            }
                            catch (Exception ex)
                            {
                                ViewBag.Status = "DB ERROR!" + ex.Message;
                            }
                            return View(); // Display modal to say category created
                        }
                        else
                        {
                            ViewBag.Status = "ERROR! Model Invalid";
                        }
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