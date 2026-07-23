using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Gdekor.Pages.NyitoOldalak
{
    public class BejelentkezesModel : PageModel
    {
        private readonly SignInManager<UserProfil> _signInManager;

        public BejelentkezesModel(SignInManager<UserProfil> signInManager)
        {
            _signInManager = signInManager;
        }

        public bool Emlekezz { get; set; }

        [BindProperty]
        [Required(ErrorMessage ="e-mail megáda kötelező !")]
        public string Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage ="jelszó megadása köelező !")]

        public string Jelszo { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var result = await _signInManager.PasswordSignInAsync(Email, Jelszo, isPersistent: false, lockoutOnFailure: true);

            if (result.Succeeded)
                return RedirectToPage("/FelhOldalak/Fomenu");

            ModelState.AddModelError(string.Empty, "Hibás email-jelszó kombináció!");
            return Page();
        }
        public void OnGet()
        {
        }
    }
}
