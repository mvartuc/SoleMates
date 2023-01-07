using System.ComponentModel.DataAnnotations;

namespace SoleMates.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Enter a valid e-mail!")]
        [Required(ErrorMessage = "Email is required!")]
        public string EmailAddress { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password, ErrorMessage = "Password must have at least 3 characters")]
        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password, ErrorMessage = "Password must have at least 3 characters")]
        [Required(ErrorMessage = "Confirm Password is required!")]
        [Compare("Password", ErrorMessage = "Passwords do not match!")]
        public string ConfirmPassword { get; set; }

    }
}
