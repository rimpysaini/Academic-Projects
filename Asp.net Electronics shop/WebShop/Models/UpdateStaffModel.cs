using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Models
{
    public class UpdateStaffModel
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "first name is required!")]
        [RegularExpression("[A-Za-z ]+")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name is required!")]
        [RegularExpression("[A-Za-z ]+", ErrorMessage = "Invalid character")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Invalid Email Address!")]
        public string Email { get; set; }

        public Active UserState { get; set; }

        public Role UserRole { get; set; }

    }
}