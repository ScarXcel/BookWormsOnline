using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Metadata.Ecma335;

namespace BookwormsOnline.Pages
{

    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public string DecryptedCreditCard { get; private set; }
        public string EncryptedCreditCard { get; private set; }
        public string SessionId { get; private set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            var encryptedCreditCard = HttpContext.Session.GetString("EncryptedCreditCard");
            EncryptedCreditCard = encryptedCreditCard;
            if (!string.IsNullOrEmpty(encryptedCreditCard))
            {
                var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                var protector = dataProtectionProvider.CreateProtector("MySecretKey");
                DecryptedCreditCard = protector.Unprotect(encryptedCreditCard);
            }
            SessionId = HttpContext.Session.Id;
        }


    }

}