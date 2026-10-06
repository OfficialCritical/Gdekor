using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

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
        public string Datum_Edit { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "elengedhetetlen a projekt kiválasztása")]
        public string? Projekt_Id_Edit { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "kötelező a munkaidő kezdetének a megadás")]
        public string? Munka_Kezdete_Edit { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "kötelező a munkaidő végének a megadás")]
        public string? Munka_Vege_Edit { get; set; } = "";
        /*
        [BindProperty]
        public string? Szunetek_Json { get; set; }
        */
        #endregion
        /*
        public class SzunetDto
        {
            public string Kezdete { get; set; }
            public string Vege { get; set; }
        }
        */
        public async Task OnGet()
        {
            await ListaBetolt();
        }

        public async Task<IActionResult> OnPostMentesAsync()
        {
            if (!ModelState.IsValid)
            {
                await ListaBetolt();
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToPage("/NyitoOldalak/Bejelentkezes");


            var projekt = await _dbcontext.Projektek_Tbl
                .FirstOrDefaultAsync(p => p.Pro_ID == Projekt_Id_Edit);

            if (projekt == null)
            {
                ModelState.AddModelError(string.Empty, "Kiválasztott projekt nem található!");
                await ListaBetolt();
                return Page();
            }
                        

            Munka_Ora munka_Ora = new Munka_Ora
            {
                MOra_ID = Guid.NewGuid().ToString(),
                M_ProID = Projekt_Id_Edit,
                M_UserID = user.Id,
                M_Nap=Datum_Edit,
                M_Kezdete = Munka_Kezdete_Edit,
                M_Vege = Munka_Vege_Edit,
                Letrehoz = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };            
            
            _dbcontext.Munkaorak_Tbl.Add(munka_Ora);

            /*
            //SZUNET
            var szunetek = string.IsNullOrWhiteSpace(Szunetek_Json) ? new List<SzunetDto>() : JsonSerializer.Deserialize<List<SzunetDto>>(Szunetek_Json) ?? new();

            foreach (var sz in szunetek)
            {
                var ujszunet = new Munka_Szunet()
                {
                    M_Nap_ID = munkanap_ID,
                    MSzunet_ID = Guid.NewGuid().ToString(),
                    Sz_Kezdete = sz.Kezdete,
                    Sz_Vege = sz.Vege
                };
                _dbcontext.Add(ujszunet);
            }            
            */
            await _dbcontext.SaveChangesAsync();

            TempData["SikerUzenet"] = "Sikeres mentés";
            return RedirectToPage();
        }
        public async Task<IActionResult> OnGetBetoltesAsync(string datum, string projektId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            if (string.IsNullOrWhiteSpace(datum) || string.IsNullOrWhiteSpace(projektId))
                return BadRequest();            
            
            var legutobbiOra = await _dbcontext.Munkaorak_Tbl
                .Where(o => o.M_ProID == projektId && o.M_UserID == user.Id && o.M_Nap == datum)
                .OrderByDescending(o => o.Letrehoz)
                .FirstOrDefaultAsync();

            if (legutobbiOra == null)
            {
                // Munkanap van, de még nincs óra — ritka, de kezeld
                return new JsonResult(new
                {
                    vanMentett = false,
                    datum = datum,
                    kezdete = (string?)null,
                    vege = (string?)null
                });
            }

            return new JsonResult(new
            {
                vanMentett = true,
                datum = datum,
                kezdete = legutobbiOra.M_Kezdete,
                vege = legutobbiOra.M_Vege
            });
        }
        public async Task ListaBetolt()
        {
            Projektek_Lista = await _dbcontext.Projektek_Tbl
                .OrderBy(p => p.Pro_Nev)
                .ToListAsync();
        }
        //vege
    }
}
