using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Gdekor.Pages.Supreme
{
    public class JelszoKezelesModel : PageModel
    {

        private readonly UserManager<UserProfil> _userMngr;
        public List<UserProfil> Profilok_Lst = new();
        public JelszoKezelesModel(UserManager<UserProfil> userMngr)
        {
            _userMngr = userMngr;
        }



        [BindProperty]
        public string Felh_id { get; set; }

        [BindProperty]
        public string Felh_nev { get; set; }

        [BindProperty]
        [Required(ErrorMessage ="jelszó megadása kötelező!")]
        public string Uj_jelszo { get; set; }



        public async Task OnGetAsync()
        {
            Profilok_Lst = await _userMngr.Users
                .Where(u=>u.Email !="kerberosz@gmail.com")
                .OrderBy(u => u.Nev)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Profilok_Lst = await _userMngr.Users
                    .Where(u => u.Email != "kerberosz@gmail.com")
                    .OrderBy(u => u.Nev)
                    .ToListAsync();

                return Page();
            }

            var user = await _userMngr.FindByIdAsync(Felh_id);

            if (user== null)
            {
                TempData["Hiba"] = "Felhasználó nem található.";
                return RedirectToPage();
            }

            var token = await _userMngr.GeneratePasswordResetTokenAsync(user);

            var result = await _userMngr.ResetPasswordAsync(user, token, Uj_jelszo);

            if (!result.Succeeded)
            {
                TempData["Hiba"] = "Hiba történt.";
                return RedirectToPage();
            }

            TempData["Siker"] = "Jelszó sikeresen módositva.";
            return RedirectToPage();
        }
    }
}
