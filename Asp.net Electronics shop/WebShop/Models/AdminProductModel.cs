using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Models
{
    public class AdminProductModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required!")]
        [RegularExpression("[A-Za-z0-9 ]+$", ErrorMessage = "Invalid character detected!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [RegularExpression(@"^\d{0,8}(\.\d{0,2})?$",ErrorMessage ="Invalid amount!")]
        public decimal Price { get; set; }

        [RegularExpression("[0-9]+$", ErrorMessage = "Invalid discount!")]
        public decimal Discount { get; set; }

        [Required(ErrorMessage = "Image is required")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase Image { get; set; }

        public List<DescriptionModel> Details { get; set; }

        [Display(Name="Category")]
        [Required(ErrorMessage = "CategoryId is required!")]
        public int CategoryID { get; set; }

        [Display(Name = "Supplier")]
        [Required(ErrorMessage = "SupplierID is required!")]
        public int SupplierID { get; set; }

    }
}