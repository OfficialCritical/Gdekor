using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Gdekor.Pages.G_Oldalak
{
    [Authorize(Roles =Szerepkorok.Admin)]
    public class ProjektekModel : PageModel
    {
        private readonly AppDbContext _dbContext;
        public ProjektekModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public List<Projekt> Projektek_Lista { get; set; } = new();
        public List<UserProfil> Userek_Lista { get; set; } = new();       
        public List<Resztvevo_Projektben> Resztvevok_Lista { get; set; } = new();       


        #region Projekt_Mentes_Props
        [BindProperty]
        public string? Resztvevok_Json { get; set; }
        public class ResztvevoDto
        {
            public string? Id { get; set; }
            public string? UserId { get; set; }
            public string? Nev { get; set; }
            public string? Oraber { get; set; }
            public string? Napiber_Ft { get; set; }
            public string? Napiber_Ora { get; set; }
        }

        [BindProperty]
        public string? Pro_ID_Edit { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "név megadása kötelező")]
        public string Nev_Edit { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "az állapot megadása kötelező")]
        public string Allapot_Edit { get; set; } = "";

        [BindProperty]        
        public string? Leir_Edit { get; set; } = "";

        /*[BindProperty]
        [Required(ErrorMessage = "a résztvevők megadása kötelező")]
        public string KikDolgoznak_Edit { get; set; } = "";
        */

        [BindProperty]
        public List<Resztvevo_Projektben> ReszvevokList_Edit { get; set; } = new();
        [BindProperty]        
        public string? TervKezd_Edit { get; set; } = "";

        [BindProperty]
        public string? TervVeg_Edit { get; set; } = "";

        [BindProperty]
        public string? ValosKezd_Edit { get; set; } = "";

        [BindProperty]
        public string? ValosVeg_Edit { get; set; } = "";

        [BindProperty]
        [RegularExpression(@"^\d*$", ErrorMessage = "bevétel csak szám lehet")]
        public string? Bevetel_Edit { get; set; } = "";

        [BindProperty]
        [RegularExpression(@"^\d*$", ErrorMessage = "költség csak szám lehet")]
        public string? Koltseg_Edit { get; set; } = "";

        [BindProperty]
        [RegularExpression(@"^\d*$", ErrorMessage = "profit csak szám lehet")]
        public string? Profit_Edit { get; set; } = "";

        #endregion


        public async Task OnGet()
        {
            await Frissit_Pro_Lista();

            Userek_Lista = await _dbContext.Users
                .OrderBy(u => u.Nev)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostMentesAsync()
        {
            if (!ModelState.IsValid)
            {
                await Frissit_Pro_Lista();
                return Page();
            }

            Projekt projekt;

            if (string.IsNullOrWhiteSpace(Pro_ID_Edit))
            {
                projekt = new Projekt
                {
                    Pro_ID = Guid.NewGuid().ToString()
                };
                _dbContext.Projektek_Tbl.Add(projekt);
            }
            else
            {
                projekt = await _dbContext.Projektek_Tbl
                    .FirstOrDefaultAsync(p => p.Pro_ID == Pro_ID_Edit);

                if (projekt==null)
                {
                    ModelState.AddModelError(string.Empty, "Kiválasztott projekt nem található!");
                    await Frissit_Pro_Lista();
                    return Page();
                }
            }

            projekt.Pro_Nev = Nev_Edit.Trim();
            projekt.Pro_Allapot = Allapot_Edit;
            projekt.Pro_Leir = Leir_Edit?.Trim();
            projekt.Pro_Terv_Kezdet = TervKezd_Edit?.Trim();
            projekt.Pro_Terv_Veg = TervVeg_Edit?.Trim();
            projekt.Pro_Valos_Kezdet = ValosKezd_Edit?.Trim();
            projekt.ProValos_Veg = ValosVeg_Edit?.Trim();
            projekt.Pro_Bevetel = Bevetel_Edit?.Trim();
            projekt.Pro_Koltseg = Koltseg_Edit?.Trim();
            projekt.Pro_Profit = Profit_Edit?.Trim();

            ResztvevokMentes(projekt.Pro_ID);

            await _dbContext.SaveChangesAsync();

            TempData["SikerUzenet"] = string.IsNullOrWhiteSpace(Pro_ID_Edit)
                ? "Új projekt sikeresen mentve."
                : "Projekt adatai sikeresen frissültek";

            return RedirectToPage();
        }

        private async Task Frissit_Pro_Lista()
        {
            Projektek_Lista = await _dbContext.Projektek_Tbl
                .OrderBy(pr => pr.Pro_Nev)
                .ToListAsync();            
        }

        private async Task Frissit_vUserek_Lista(string proID)
        {
            var proResztvevok = await _dbContext.ResztvevokProBen_Tbl
                .Where(u => u.Pro_ID == proID)
                .Select(u=>u.User_ID)
                .ToListAsync();
            
            Userek_Lista = await _dbContext.Users
                .Where(u => !proResztvevok.Contains(u.Id))
                .OrderBy(u=>u.Nev)
                .ToListAsync();            
        }

        private void ResztvevokMentes(string proId)
        {
            var regiek = _dbContext.ResztvevokProBen_Tbl
                .Where(r => r.Pro_ID == proId);
            _dbContext.ResztvevokProBen_Tbl.RemoveRange(regiek);

            var dtoLista = string.IsNullOrWhiteSpace(Resztvevok_Json) ? new List<ResztvevoDto>() : JsonSerializer.Deserialize<List<ResztvevoDto>>(Resztvevok_Json,new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();

            foreach (var r in dtoLista)
            {
                if (string.IsNullOrWhiteSpace(r.Nev) && string.IsNullOrWhiteSpace(r.UserId))
                    continue;
                _dbContext.ResztvevokProBen_Tbl.Add(new Resztvevo_Projektben
                {
                    ID = Guid.NewGuid().ToString(),
                    Pro_ID = proId,
                    User_ID = r.UserId,
                    Nev = r.Nev?.Trim(),
                    Oraber = r.Oraber?.Trim(),
                    Napiber_Ft = r.Napiber_Ft?.Trim(),
                    Napiber_Ora = r.Napiber_Ora?.Trim()
                });
            }

        }

        public async Task<IActionResult> OnGetResztvevokAsync(string proId)
        {
            if (string.IsNullOrWhiteSpace(proId))
                return BadRequest();

            var resztv_list = await _dbContext.ResztvevokProBen_Tbl
                .Where(r => r.Pro_ID == proId)
                .Select(r=> new
                {
                    id=r.ID,
                    userId= r.User_ID,
                    nev = r.Nev,
                    oraber= r.Oraber,
                    napiber_Ft= r.Napiber_Ft,
                    napiber_Ora=r.Napiber_Ora
                })
                .ToListAsync();

            var munkaorak = await _dbContext.Munkaorak_Tbl
                .Where(m => m.M_ProID == proId)
                .ToListAsync();

            var ugyeskedok = munkaorak
                .GroupBy(m => new { m.M_UserID, m.M_Nap })
                .Where(g => g.Count() > 1)
                .Select(g => new
                {
                    userId = g.Key.M_UserID,
                    nap = g.Key.M_Nap,
                    adatok = g.OrderBy(m => DateTime.Parse(m.Letrehoz!))
                    .Select(m => new
                    {
                        kezdet = m.M_Kezdete,
                        veg = m.M_Vege,
                        letrehoz = m.Letrehoz
                    })
                    .ToList()
                })
                .ToList();

            var ugyesUserIds = ugyeskedok
                .Select(u => u.userId)
                .Distinct()
                .ToList();

            var ugyesUserNevek = await _dbContext.Users
                .Where(u => ugyesUserIds.Contains(u.Id))
                .ToDictionaryAsync(
                    u => u.Id,
                    u => u.Nev
                );

            var ugyeskedok_List = ugyeskedok
                .GroupBy(u => u.userId)
                .Select(userGroup => new
                {
                    userId = userGroup.Key,

                    nev = ugyesUserNevek.TryGetValue(userGroup.Key, out var nev)
                        ? nev
                        : "Ismeretlen",

                    napok = userGroup
                        .Select(u => new
                        {
                            nap = u.nap,
                            adatok = u.adatok
                        })
                        .ToList()
                })
                .ToList();

            //oraszamok
            var legutolsoOrak = munkaorak
                .GroupBy(m => new { m.M_UserID, m.M_Nap })
                .Select(g => g
                    .OrderByDescending(m => DateTime.Parse(m.Letrehoz!))
                    .First())
                .ToList();

            var oraszamok_List = new List<object>();

            foreach (var resztvevo in resztv_list)
            {
                var userOrai = legutolsoOrak
                    .Where(m => m.M_UserID == resztvevo.userId)
                    .ToList();

                double osszOra = 0;
                double tulora = 0;

                double.TryParse(resztvevo.napiber_Ora, out double maxNapiOra);

                foreach (var munka in userOrai)
                {
                    if (!TimeSpan.TryParse(munka.M_Kezdete, out var kezdet) || !TimeSpan.TryParse(munka.M_Vege, out var veg))
                        continue;

                    double napiOra = (veg - kezdet).TotalHours;

                    osszOra += napiOra;

                    if (maxNapiOra > 0 && napiOra > maxNapiOra)
                    {
                        tulora += napiOra - maxNapiOra;
                    }
                }

                string fizetes="?";

                if (double.TryParse(resztvevo.oraber, out double oraber))
                {
                    fizetes = Math.Round(osszOra * oraber).ToString();
                }
                else if (double.TryParse(resztvevo.napiber_Ft, out double napiber) && double.TryParse(resztvevo.napiber_Ora,out double maxOra) && maxOra > 0)
                {
                    double calcOraber = napiber / maxOra;
                    fizetes = Math.Round(osszOra * calcOraber).ToString();
                }

                oraszamok_List.Add(new
                {
                    userId = resztvevo.userId,
                    nev = resztvevo.nev,
                    osszOra = Math.Round(osszOra, 2),
                    tulora=Math.Round(tulora,2),
                    fizetes=fizetes
                });
            }

            return new JsonResult(new
            {
                resztvevok = resztv_list,
                oraszamok = oraszamok_List,
                ugyeskedok= ugyeskedok_List
            });
        }

        //public async Task<IActionResult> OnGetOraszamokAsync(string proId)
        //vege
    }
}
