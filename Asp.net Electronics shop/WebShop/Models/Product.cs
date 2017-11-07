using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebShop.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string Description { get; set; }
        public int CatID { get; set; }
        public int SupID { get; set; }
        public string ImageURL { get; set; }
        public string SupplierName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}