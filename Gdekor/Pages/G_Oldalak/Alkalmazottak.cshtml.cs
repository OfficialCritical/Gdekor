using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Gdekor.Pages.G_Oldalak
{
    public class AlkalmazottakModel : PageModel
    {
        private readonly AppDbContext _appContext;
        private readonly UserManager<UserProfil> _userManager;
        

        public AlkalmazottakModel(AppDbContext appDbContext,UserManager<UserProfil> userManager)
        {
            _userManager = userManager;
            _appContext= appDbContext;
        }

        public List<UserProfil> Felhasznalok_Lst;
        public async  void OnGet()
        {
            Felhasznalok_Lst = await _appContext.Users
                .OrderBy(u => u.Nev)
                .ToListAsync();
        }
    }
}
