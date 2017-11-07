using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Models
{
    public class ProductModel
    {
        [Display(Name = "Name of Product")]
        [Required]
        [RegularExpression("[A-Za-z]+")]
        public string Name { get; set; }

        [Display(Name = "Price of Product")]
        [Required]
        [RegularExpression("[0-9]+(\\.[0-9][0-9]?)?")]
        public decimal Price { get; set; }

        [Display(Name = "Discount on Product")]
        [Required]
        [RegularExpression("[0-9]+(\\.[0-9][0-9]?)?")]
        public decimal Discount { get; set; }

        [Display(Name = "Description of Product")]
        [RegularExpression("[A-Za-z]+")]
        public string Description { get; set; }

        [Display(Name = "Category of product")]
        [Required]
        [RegularExpression("[0-9]+")]
        public int CatID { get; set; }

        [Display(Name = "Supplier of product")]
        [Required]
        [RegularExpression("[0-9]+")]
        public int SupID { get; set; }
    }
}