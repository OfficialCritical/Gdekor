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


            var munkanap_ID = Guid.NewGuid().ToString();

            Munkanap munka_Nap;
            Munka_Ora munka_Ora = new Munka_Ora();

            // NAP
            if (!string.IsNullOrWhiteSpace(MNap_ID_Edit))
            {
                munka_Nap = await _dbcontext.Munkanaptok_Tbl.FirstOrDefaultAsync(mn => mn.MNap_ID == MNap_ID_Edit && mn.Prof_ID == user.Id && mn.Projekt_ID == Projekt_Id_Edit);

                if (munka_Nap == null)
                {
                    ModelState.AddModelError(string.Empty, "Hiba történt az elmentett munkanap megkeresésekor!");
                    await ListaBetolt();
                    return Page();
                }
                munkanap_ID = MNap_ID_Edit;

            }
            else
            {
                munka_Nap = new Munkanap
                {
                    MNap_ID = munkanap_ID,
                    Projekt_ID = Projekt_Id_Edit,
                    Prof_ID = user.Id,
                    M_Datum = Datum_Edit
                };
                _dbcontext.Munkanaptok_Tbl.Add(munka_Nap);
                //MNap_ID_Edit = munka_Nap.MNap_ID;
            }

            //ORA
            munka_Ora.M_Nap_ID = MNap_ID_Edit;
            munka_Ora.MOra_ID = Guid.NewGuid().ToString();
            munka_Ora.M_Kezdete = Munka_Kezdete_Edit;
            munka_Ora.M_Vege = Munka_Vege_Edit;
            munka_Ora.Letrehoz = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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

            // Munkanap: user + dátum + projekt
            var munkanap = await _dbcontext.Munkanaptok_Tbl
                .FirstOrDefaultAsync(m =>
                    m.M_Datum == datum &&
                    m.Prof_ID == user.Id &&
                    m.Projekt_ID == projektId);

            if (munkanap == null)
            {
                return new JsonResult(new
                {
                    vanMentett = false,
                    datum = datum,
                    mNapId = (string?)null,
                    kezdete = (string?)null,
                    vege = (string?)null
                });
            }

            // Legutolsó munkaóra (audit: több sor is lehet)
            var legutobbiOra = await _dbcontext.Munkaorak_Tbl
                .Where(o => o.M_Nap_ID == munkanap.MNap_ID)
                .OrderByDescending(o => o.Letrehoz)
                .FirstOrDefaultAsync();

            if (legutobbiOra == null)
            {
                // Munkanap van, de még nincs óra — ritka, de kezeld
                return new JsonResult(new
                {
                    vanMentett = false,
                    datum = datum,
                    mNapId = munkanap.MNap_ID,
                    kezdete = (string?)null,
                    vege = (string?)null
                });
            }

            return new JsonResult(new
            {
                vanMentett = true,
                datum = datum,
                mNapId = munkanap.MNap_ID,
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
