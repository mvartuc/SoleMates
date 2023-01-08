using System.ComponentModel.DataAnnotations;

namespace SoleMates.ViewModels
{
    public class RegisterViewModel
    {
        //[Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [Required()]
        public string EmailAddress { get; set; }

        //[Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required()]
        public string Password { get; set; }

        //[Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Required()]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

    }
}
