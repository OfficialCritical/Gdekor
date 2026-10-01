using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gdekor.Pages.FelhOldalak
{
    [Authorize(Roles = $"{Szerepkorok.Mugli},{Szerepkorok.Admin}")]
    public class FomenuModel : PageModel
    {
        private readonly UserManager<UserProfil> _userMngr;
        public FomenuModel(UserManager<UserProfil> userManager)
        {
            _userMngr= userManager;
        }


        public string? BejEmiil { get; set; }



        public async Task OnGetAsync()
        {
            var user = await _userMngr.GetUserAsync(User);
            BejEmiil = user?.Email;
        }
    }
}
