using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gdekor.Pages.FelhOldalak
{
    [Authorize(Roles = $"{Szerepkorok.Mugli},{Szerepkorok.Admin}")]
    public class FomenuModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
