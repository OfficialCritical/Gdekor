using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Gdekor.Pages.FelhOldalak
{
    [Authorize(Roles = $"{Szerepkorok.Mugli},{Szerepkorok.Admin}")]
    public class MunkaoraimModel : PageModel
    {
        private readonly AppDbContext _dbcontext;
        private readonly UserManager<UserProfil> _userManager;
        public MunkaoraimModel(AppDbContext appDbContext, UserManager<UserProfil> userManager)
        {
            _dbcontext = appDbContext;
            _userManager = userManager;
        }

        public List<Projekt> Projektek_Lista { get; set; } = new();



        #region Mentesi_Props

        [BindProperty]
        public string? MNap_ID_Edit { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Válassz dátumot!")]
        public string Datum_Edit { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "elengedhetetlen a projekt kiválasztása")]
        public string? Projekt_Id_Edit { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage ="kötelező a munkaidő kezdetének a megadás")]
        public string? Munka_Kezdete_Edit { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "kötelező a munkaidő végének a megadás")]
        public string? Munka_Vege_Edit { get; set; } = "";


        #endregion


        public async Task OnGet()
        {
            Projektek_Lista = await _dbcontext.Projektek_Tbl
                .OrderBy(p => p.Pro_Nev)
                .ToListAsync();
        }
        
        public async Task<IActionResult> OnPostMentesAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = _userManager.GetUserAsync(User);

            var projekt = await _dbcontext.Projektek_Tbl
                .FirstOrDefaultAsync(p => p.Pro_ID == Projekt_Id_Edit);

            if(projekt == null)
            {
                ModelState.AddModelError(string.Empty, "Kiválasztott projekt nem található!");
                return Page();
            }

            return RedirectToPage();
        }
    }
}
