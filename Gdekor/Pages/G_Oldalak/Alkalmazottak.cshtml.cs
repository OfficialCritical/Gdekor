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
            Felhasznalok_Lst = await _userManager.Users
                .OrderBy(u => u.Nev)
                .ToListAsync();
        }
    }
}
