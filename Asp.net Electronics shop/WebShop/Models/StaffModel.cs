using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Models
{
    public class StaffModel
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "first name is required!")]
        [RegularExpression("[A-Za-z ]+")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name is required!")]
        [RegularExpression("[A-Za-z ]+",ErrorMessage ="Invalid character")]
        public string LastName { get; set; }

        [Required(ErrorMessage ="Email is required!")]
        [EmailAddress(ErrorMessage = "Invalid Email Address!")]
        public string Email { get; set; }

        public Active UserState { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [StringLength(10, ErrorMessage =
            "Password must be 5 to 10 characters long",
            MinimumLength = 5)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm password is required!")]
        [StringLength(10, ErrorMessage =
            "Password must be 5 to 10 characters long",
            MinimumLength = 5)]
        [Compare("Password",ErrorMessage ="Password not matching!")]
        public string ComparePassword { get; set; }

        public Role UserRole { get; set; }
    }
}