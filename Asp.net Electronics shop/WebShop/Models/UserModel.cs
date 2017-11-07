using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Models
{
    public class UserModel
    {
        [Display(Name = "First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name required")]
        [RegularExpression("[A-Za-z]+")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name required")]
        [RegularExpression("[A-Za-z]+")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email required")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Phone")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Phone Number is required")]
        public string Phone { get; set; }

        [Display(Name = "Address Line One:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Address is required")]
        public string Street1 { get; set; }

        [Display(Name = "Address Line Two: ")]
        public string Street2 { get; set; }

        [Display(Name = "Town")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Town is required")]
        public string Town { get; set; }

        [Display(Name = "County")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "County is required")]
        public string County { get; set; }


        public DateTime DateRegistered { get; set; }


        public int IsActivated { get; set; }


        public DateTime LastLogin { get; set; }


        public int Logged { get; set; }

        public int RoleID { get; set; }

        [Display(Name = "Security Question")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Security Question Required")]
        public string SecurityQuestion { get; set; }

        [Display(Name = "Answer for your security question")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Security Answer required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum 6 characters required for valid answer to security question")]
        public string Answer { get; set; }

        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum 6 characters required for valid Password")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password not matched")]
        public string ConfirmPassword { get; set; }
    }
}