using System.ComponentModel.DataAnnotations;

namespace Company.PL.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Password does not match")]
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
