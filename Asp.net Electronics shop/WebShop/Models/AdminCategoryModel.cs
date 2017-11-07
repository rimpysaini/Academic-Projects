using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Models
{
    public class AdminCategoryModel
    {
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression("[A-Za-z ]+", ErrorMessage = "Invalid character detected!")]
        public string Name { get; set; }

        [RegularExpression("[A-Za-z ]+",ErrorMessage ="Invalid character detected!")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Image is required")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase Image{ get; set; }
    }
}