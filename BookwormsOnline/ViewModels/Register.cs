using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace BookwormsOnline.ViewModels
{
    public class Register
    {
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Credit Card number must be 16 digits")]
        public string CreditCard { get; set; }

        [Required]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "Mobile Number must be 8 digits")]
        public string MobileNo { get; set; }

        [Required]
        public string Billing { get; set; }


        [Required(ErrorMessage = "Shipping Address is required")]
        [MaxLength(200, ErrorMessage = "Shipping Address cannot be longer than 200 characters")]
        public string Shipping { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(12, ErrorMessage = "Enter at least a 12 characters password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%?&])[A-Za-z\d$@$!%?&]{12,}$", ErrorMessage = "Password must be at least 12 characters long and contain at least an uppercase letter, lower case letter, digit and a symbol")]

        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
        public string ConfirmPassword { get; set; }

    }
}
