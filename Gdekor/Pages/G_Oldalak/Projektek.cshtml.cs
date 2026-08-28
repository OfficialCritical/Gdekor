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


        #region Projekt_Mentes_Props
        [BindProperty]
        public string? Resztvevok_Json { get; set; }
        public class ResztvevoDto
        {
            public string? Id { get; set; }
            public string? UserId { get; set; }
            public string? Nev { get; set; }
            public string? Oraber { get; set; }
            public string? Napiber { get; set; }
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

        [BindProperty]
        [Required(ErrorMessage = "a résztvevők megadása kötelező")]
        public string KikDolgoznak_Edit { get; set; } = "";

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
            await ListaToltesAsync();
        }

        public async Task<IActionResult> OnPostMentesAsync()
        {
            if (!ModelState.IsValid)
            {
                await ListaToltesAsync();
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
                    await ListaToltesAsync();
                    return Page();
                }
            }

            projekt.Pro_Nev = Nev_Edit.Trim();
            projekt.Pro_Allapot = Allapot_Edit;
            projekt.Pro_Leir = Leir_Edit?.Trim();
            projekt.Pro_KikDolgoznak = KikDolgoznak_Edit?.Trim();
            projekt.Pro_Terv_Kezdet = TervKezd_Edit?.Trim();
            projekt.Pro_Terv_Veg = TervVeg_Edit?.Trim();
            projekt.Pro_Valos_Kezdet = ValosKezd_Edit?.Trim();
            projekt.ProValos_Veg = ValosVeg_Edit?.Trim();
            projekt.Pro_Bevetel = Bevetel_Edit?.Trim();
            projekt.Pro_Koltseg = Koltseg_Edit?.Trim();
            projekt.Pro_Profit = Profit_Edit?.Trim();

            await ResztvevokMentesAsync(projekt.Pro_ID);

            await _dbContext.SaveChangesAsync();

            TempData["SikerUzenet"] = string.IsNullOrWhiteSpace(Pro_ID_Edit)
                ? "Új projekt sikeresen mentve."
                : "Projekt adatai sikeresen frissültek";

            return RedirectToPage();
        }

        private async Task ListaToltesAsync()
        {
            Projektek_Lista = await _dbContext.Projektek_Tbl
                .OrderBy(pr => pr.Pro_Nev)
                .ToListAsync();

            foreach (var projekt in Projektek_Lista)
            {
                Console.WriteLine(
                    $"{projekt.Pro_Nev} | " +
                    $"TervKezd: {projekt.Pro_Terv_Kezdet} | " +
                    $"TervVeg: {projekt.Pro_Terv_Veg} | " +
                    $"Bevetel: {projekt.Pro_Bevetel} | " +
                    $"Koltseg: {projekt.Pro_Koltseg} | " +
                    $"Profit: {projekt.Pro_Profit}"
                );
            }

            Userek_Lista = await _dbContext.Users
                .OrderBy(u => u.Nev)
                .ToListAsync();


        }

        private async Task ResztvevokMentesAsync(string proId)
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
                    Napiber = r.Napiber?.Trim()
                });
            }
            await Task.CompletedTask;

        }

        public async Task<IActionResult> OnGetResztvevokAsync(string proId)
        {
            if (string.IsNullOrWhiteSpace(proId))
                return BadRequest();

            var lista = await _dbContext.ResztvevokProBen_Tbl
                .Where(r => r.Pro_ID == proId)
                .Select(r=> new
                {
                    id=r.ID,
                    userId= r.User_ID,
                    nev = r.Nev,
                    oraber= r.Oraber,
                    napiber= r.Napiber
                })
                .ToListAsync();

            return new JsonResult(lista);
        }
        //vege
    }
}
