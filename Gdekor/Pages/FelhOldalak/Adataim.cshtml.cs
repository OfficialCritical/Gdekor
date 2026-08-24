using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace Gdekor.Pages.FelhOldalak
{
    [Authorize(Roles =$"{Szerepkorok.Mugli},{Szerepkorok.Admin}")]
    public class AdataimModel : PageModel
    {

        private readonly UserManager<UserProfil> _userManager;
        private readonly IWebHostEnvironment _env;

        #region Props
        [BindProperty]
        [Required(ErrorMessage = "e-mail megadása kötelező")]
        public string EditEmail {  get; set; } = "";

        [BindProperty]        
        public string? EditJelszo { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "név megadása kötelező")]
        public string EditNev { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "személyi szám megadása kötelező")]
        public string EditSzemelyi { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "lakcím kártya számának megadása kötelező")]
        public string EditLakcKartyaSzam { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "adószám megadása kötelező")]
        public string EditAdoszam { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "tajszám megadása kötelező")]
        public string EditTajszam { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "körzetszám megadása kötelező")]
        public string EditTel_Korzet {  get; set; }

        [BindProperty]
        [Required(ErrorMessage = "telefonszám megadása kötelező")]
        [RegularExpression(@"^\d{7}$", ErrorMessage = "az előfizetői számnak 7 számjegyből kell állnia")]
        public string EditTel_Szemelyi { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage ="lakcím megadása kötelező")]
        public string EditLakcim { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "születési hely megadása kötelező")]
        public string EditSzulHely { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "születési idő megadása kötelező")]
        public string EditSzulIdo { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "anyja nevének megadása kötelező")]
        public string EditAnyjaNeve { get; set; } = "";

        public string? AktualProfKep {  get; set; } 

        [BindProperty]
        public IFormFile? EditProfKep { get; set; }


        [BindProperty]
        public string? EditJogsiSzama { get; set; }
        [BindProperty]
        public string? EditJogsiKateg { get; set; }
        [BindProperty]
        public string? EditRendszam { get; set; } = "";

        [BindProperty]
        public string? EditFerohely { get; set; } = "";
        #endregion


        public AdataimModel(UserManager<UserProfil> userManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _env = env;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            //if (user == null) return RedirectToPage("/NyitoOldalak/Bejelentkezes");

            EditEmail = user.Email ?? "";
            EditNev = user.Nev ?? "";
            EditSzemelyi = user.SzemelyiSzam ?? "";
            EditTel_Korzet = user.Tel_Korzet ?? "";
            EditTel_Szemelyi = user.PhoneNumber ?? "";
            EditLakcKartyaSzam = user.LakcimK_Szama ?? "";
            EditAdoszam = user.Adoszam ?? "";
            EditTajszam = user.Tajszam ?? "";
            EditLakcim = user.Lakcim ?? "";
            EditSzulHely = user.SzulHely ?? "";
            EditSzulIdo = user.SzulIdo ?? "";
            EditAnyjaNeve = user.AnyjaNeve ?? "";
            AktualProfKep = user.ProfKepUtovnal ?? "";
            EditJogsiSzama = user.JogsiSzama ?? "";
            EditJogsiKateg = user.JogsiKateg ?? "";
            EditRendszam = user.Rendszam ?? "";
            EditFerohely = user.Ferohely ?? "";
            EditJelszo = null;  // jelszó sosem töltődik vissza

            return Page();
        }
        
        public async Task<IActionResult> OnPostAdataimAsync()
        {
            if (!ModelState.IsValid)
                return Page();

           
            var user = await _userManager.GetUserAsync(User);
            if(user == null) return RedirectToPage("/NyitoOldalak/Bejelentkezes");

            user.Email = EditEmail.Trim();
            user.UserName = EditEmail.Trim();
            user.Nev = EditNev.Trim();
            user.SzemelyiSzam = EditSzemelyi.Trim();
            user.LakcimK_Szama=EditLakcKartyaSzam.Trim();
            user.Adoszam=EditAdoszam.Trim();
            user.Tajszam=EditTajszam.Trim();
            user.Tel_Korzet = EditTel_Korzet.Trim();
            user.PhoneNumber = EditTel_Szemelyi.Trim();
            user.Lakcim = EditLakcim.Trim();
            user.SzulHely = EditSzulHely.Trim();
            user.SzulIdo = EditSzulIdo.Trim();
            user.AnyjaNeve = EditAnyjaNeve.Trim();
            user.JogsiSzama = EditJogsiSzama?.Trim();
            user.JogsiKateg = EditJogsiKateg?.Trim();
            user.Rendszam = EditRendszam?.Trim();
            user.Ferohely = EditFerohely?.Trim();

            if (!string.IsNullOrWhiteSpace(EditJelszo))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result= await _userManager.ResetPasswordAsync(user, token, EditJelszo);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        return Page();
                    }
                }
            }
            
            if (EditProfKep != null && EditProfKep.Length>0)
            {
                var engedelyezett = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var kiterjesztes = Path.GetExtension(EditProfKep.FileName).ToLowerInvariant();

                if (!engedelyezett.Contains(kiterjesztes))
                {
                    ModelState.AddModelError(nameof(EditProfKep), "Csak JPG, PNG vagy WEBP fájl tölthető fel.");
                    return Page();
                }

                if (EditProfKep.Length> 2*1024*1024)//2mb
                {
                    ModelState.AddModelError(nameof(EditProfKep), "Maximum 2 MB lehet a kép!");
                    return Page();
                }

                var profKepMappa = Path.Combine(_env.WebRootPath, "images", "profKepek");
                Directory.CreateDirectory(profKepMappa);

                if (!string.IsNullOrEmpty(user.ProfKepUtovnal))
                {
                    var regiUtvonal = Path.Combine(_env.WebRootPath, user.ProfKepUtovnal.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if ((System.IO.File.Exists(regiUtvonal)))
                        System.IO.File.Delete(regiUtvonal);
                }

                var ujFajlNev = $"{user.Id}_{Guid.NewGuid()}{kiterjesztes}";
                var mentesiVonal = Path.Combine(profKepMappa,ujFajlNev);

                using (var stream = new FileStream(mentesiVonal, FileMode.Create))
                {
                    await EditProfKep.CopyToAsync(stream);
                }
                user.ProfKepUtovnal = $"/images/profKepek/{ujFajlNev}";
            }
            

            var f_result = await _userManager.UpdateAsync(user);

            if (!f_result.Succeeded)
            {
                foreach (var err in f_result.Errors)
                {
                    ModelState.AddModelError(string.Empty, err.Description);
                    return Page();
                }
            }
            TempData["SikerUzenet"] = "Sikeres mentés.";
            return RedirectToPage();
            
        }
    }
}
