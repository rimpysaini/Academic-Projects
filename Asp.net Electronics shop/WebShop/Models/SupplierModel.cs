using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Models
{
    public class SupplierModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The name is required!")]
        [RegularExpression("[A-Za-z ]+$", ErrorMessage = "Invalid character detected!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The address is required!")]
        [RegularExpression("[A-Za-z0-9 ,]+$", ErrorMessage = "Invalid character in address!")]
        public string Address { get; set; }

        [Required(ErrorMessage = "The phone is required!")]
        [Display(Name = "Phone number")]
        [RegularExpression("[0-9 ]+$", ErrorMessage = "Inavlid phone number!")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required!")]
        [EmailAddress(ErrorMessage = "Invalid Email Address!")]
        public string Email { get; set; }
    }
}