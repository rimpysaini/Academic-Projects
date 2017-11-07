using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebShop.Models
{
    public class ProductViewModel
    {
        public IEnumerable<DescriptionModel> ProductDescription { get; set; }
        public IEnumerable<AdminProduct> FullProduct { get; set; }
    }
}