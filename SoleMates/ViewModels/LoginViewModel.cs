using System.ComponentModel.DataAnnotations;

namespace SoleMates.ViewModels
{
    public class LoginViewModel
    {
        [Required()]
        [DataType(DataType.EmailAddress)]
        [Display()]
        public string EmailAddress { get; set; }

        [Required()]
        [DataType(DataType.Password)]
        [Display()]
        public string Password { get; set; }
    }
}
