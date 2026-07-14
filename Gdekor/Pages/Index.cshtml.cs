using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gdekor.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            //return RedirectToPage("/NyitoOldalak/Bejelentkezes");

            //return RedirectToPage("/FelhOldalak/Fomenu");
            //return RedirectToPage("/FelhOldalak/Munkaoraim");

            return RedirectToPage("/G_Oldalak/Projektek");
            
        }
    }
}
