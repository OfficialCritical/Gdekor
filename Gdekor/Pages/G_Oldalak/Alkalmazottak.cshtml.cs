using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Gdekor.Pages.G_Oldalak
{
    [Authorize(Roles =$"{Szerepkorok.Admin}")]
    public class AlkalmazottakModel : PageModel
    {
        private readonly AppDbContext _appContext;
        private readonly UserManager<UserProfil> _userManager;
        

        public AlkalmazottakModel(AppDbContext appDbContext,UserManager<UserProfil> userManager)
        {
            _userManager = userManager;
            _appContext= appDbContext;
        }

        public List<UserProfil> Felhasznalok_Lst = new();
        public async Task OnGetAsync()
        {
            await LoadFelhasznalokLista();
        }

        public async Task<IActionResult> OnPostTorlesAsync(string id)
        {

            if (string.IsNullOrWhiteSpace(id))
            {
                ModelState.AddModelError(string.Empty, "Hiba történt");
                await LoadFelhasznalokLista();
                return Page();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Hiba történt");
                await LoadFelhasznalokLista();
                return Page();
            }

            if (user.Email == "kerberosz@gmail.com" || user.Id == _userManager.GetUserId(User))
            {
                ModelState.AddModelError(string.Empty, "Hiba történt");
                await LoadFelhasznalokLista();
                return Page();
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Hiba történt");
                await LoadFelhasznalokLista();
                return Page();
            }

            TempData["SikerUzenet"] = "Sikeres törlés";
            return RedirectToPage();
        }

        public async Task LoadFelhasznalokLista()
        {
            Felhasznalok_Lst = await _userManager.Users
                .OrderBy(u => u.Nev)
                .ToListAsync();
        }
    }
}
