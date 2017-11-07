using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Models
{
    public class AdminProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string ImageUrl { get; set; }
        public List<DescriptionModel> Details { get; set; }
        [Display(Name = "Category Name")]
        public int CategoryID { get; set; }
        [Display(Name = "Supplier Name")]
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
    }
}