using System.ComponentModel.DataAnnotations;

namespace InventoryApp.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="First Name is Required")]
        public string FName { get; set; }
        [Required(ErrorMessage = "Last Name is Required")]
        public string LName { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage ="Invalid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is Required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Password and Confirm Password do not match")]
        public string ConfirmPassword { get; set; }

    }
}
