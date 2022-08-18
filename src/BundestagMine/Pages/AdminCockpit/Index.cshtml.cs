using BundestagMine.Data;
using BundestagMine.Services;
using BundestagMine.ViewModels.Import;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BundestagMine.Pages
{
    public class AdminCockpitModel : PageModel
    {
        private readonly ImportService _importService;
        private readonly SignInManager<IdentityUser> _signInManager;

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        [BindProperty]
        public InputModel LoginInput { get; set; }

        /// <summary>
        /// A list of import logs to list in the view.
        /// </summary>
        [BindProperty]
        public List<ImportLogViewModel> ImportList { get; set; }

        public AdminCockpitModel(SignInManager<IdentityUser> signInManager, ImportService importService)
        {
            _importService = importService;
            _signInManager = signInManager;
        }

        public async Task OnGetAsync()
        {
            //if (!User.Identity.IsAuthenticated) return;

            ImportList = _importService.GetAllImportLogFileNames();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(LoginInput.Email, LoginInput.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToPage("");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
