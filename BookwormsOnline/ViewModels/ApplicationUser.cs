using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BookwormsOnline.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CreditCard { get; set; }

        public string MobileNo { get; set; }

        public string Billing { get; set; }

        public string Shipping { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        //For session 
        public DateTime LastLoginTime { get; set; }

    }
}