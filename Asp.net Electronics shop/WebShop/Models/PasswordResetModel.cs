using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Models
{
    public class PasswordResetModel
    {
        [Required(ErrorMessage = "Paasword is required!")]
        [StringLength(10, ErrorMessage =
            "Password must be 5 to 10 characters long",
            MinimumLength = 5)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm password is required!")]
        [StringLength(10, ErrorMessage =
            "Password must be 5 to 10 characters long",
            MinimumLength = 5)]
        [Compare("Password", ErrorMessage = "Password not matching!")]
        public string ComparePassword { get; set; }

    }
}