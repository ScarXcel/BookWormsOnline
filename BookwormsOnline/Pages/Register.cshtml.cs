using BookwormsOnline.Model;
using BookwormsOnline.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookwormsOnline.Pages
{
    public class RegisterModel : PageModel
    {

        private UserManager<ApplicationUser> userManager { get; }
        private SignInManager<ApplicationUser> signInManager { get; }

        [BindProperty]
        public Register RModel { get; set; }

        public RegisterModel(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }



        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostAsync()
        {

            if (ModelState.IsValid)
            {

                var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                var protector = dataProtectionProvider.CreateProtector("MySecretKey");

                var user = new ApplicationUser()
                {
                    UserName = RModel.Email,
                    Email = RModel.Email,
                    CreditCard = protector.Protect(RModel.CreditCard),
                    FirstName = RModel.FirstName,
                    LastName = RModel.LastName,
                    MobileNo = RModel.MobileNo,
                    Billing = RModel.Billing,
                    Shipping = RModel.Shipping,


                };
                var result = await userManager.CreateAsync((ApplicationUser)user, RModel.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    HttpContext.Session.SetString("UserEmail", user.Email);

                    // Set the encrypted credit card in the session
                    var encryptedCreditCard = protector.Protect(RModel.CreditCard);
                    HttpContext.Session.SetString("EncryptedCreditCard", encryptedCreditCard);

                    return RedirectToPage("./Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }



            }
            return Page();
        }


    }
}
