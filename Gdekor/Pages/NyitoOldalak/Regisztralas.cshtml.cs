using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Gdekor.Pages.NyitoOldalak
{
    public class RegisztralasModel : PageModel
    {
        private readonly UserManager<UserProfil> _userManager;
        private readonly SignInManager<UserProfil> _signInManager;
        public RegisztralasModel(UserManager<UserProfil> userManager,SignInManager<UserProfil> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        #region regprops
        [BindProperty]
        [Required(ErrorMessage ="e-mail megadása kötelező")]
        public string RegEmail {  get; set; }

        [BindProperty]
        [Required(ErrorMessage ="jelszó megadása kötelező")]
        public string RegJelszo { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "teljes név megadása kötelező")]
        public string RegNev { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "személyi szám megadása kötelező")]
        public string RegSzemelyi { get; set; }

        [BindProperty]
        public string RegTel_Korzet {  get; set; }

        [BindProperty]
        [Required(ErrorMessage = "telefonszám megadása kötelező")]
        [RegularExpression(@"^\d{7}$",ErrorMessage ="az előfizetői számnak 7 számjegyből kell állnia")]
        public string RegTel_Egyeni { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "születési hely megadása kötelező")]
        public string RegSzulHely {  get; set; }

        [BindProperty]
        [Required(ErrorMessage = "születési idő megadása kötelező")]
        public string RegSzulIdo { get; set; }

        [BindProperty]
        [Required(ErrorMessage ="anyja nevének megadása kötelező")]
        public string RegAnyjaNeve { get; set; }

        [BindProperty]
        public IFormFile? RegProfKep { get; set; }
        #endregion

        public async Task<IActionResult> OnPostRegisztracioAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var existingUser = await _userManager.FindByEmailAsync(RegEmail);

            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "A megadott e-mail már regisztrálva van!");
                return Page();
            }

            if (RegProfKep != null && RegProfKep.Length > 0)
            {
                var engedelyezett = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var kiterjesztes = Path.GetExtension(RegProfKep.FileName).ToLowerInvariant();

                if (!engedelyezett.Contains(kiterjesztes))
                {
                    ModelState.AddModelError(nameof(RegProfKep), "Csak JPG, PNG vagy WEBP fájl tölthető fel.");
                    return Page();
                }

                if (RegProfKep.Length > 2 * 1024 * 1024)//2mb
                {
                    ModelState.AddModelError(nameof(RegProfKep), "Maximum 2 MB lehet a kép!");
                    return Page();
                }
            }

            var user = new UserProfil
            {
                UserName = RegEmail.Trim(),
                Email = RegEmail.Trim(),
                EmailConfirmed = true,
                Nev = RegNev.Trim(),
                Tel_Korzet = RegTel_Korzet.Trim(),
                PhoneNumber = RegTel_Egyeni.Trim(),                
                SzemelyiSzam = RegSzemelyi.Trim(),
                AnyjaNeve = RegAnyjaNeve.Trim(),
                SzulHely = RegSzulHely.Trim(),
                SzulIdo = RegSzulIdo.Trim()
            };


            var result = await _userManager.CreateAsync(user, RegJelszo);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _userManager.AddToRoleAsync(user, Szerepkorok.Mugli);

            if (RegProfKep != null && RegProfKep.Length > 0)
            {
                var kiterjeszteS = Path.GetExtension(RegProfKep.FileName).ToLowerInvariant();
                var ujFajlNev = $"{user.Id}_{Guid.NewGuid()}{kiterjeszteS}";
                var mentesiVonal = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "profKepek", ujFajlNev);

                Directory.CreateDirectory(Path.GetDirectoryName(mentesiVonal)!);

                using (var stream = new FileStream(mentesiVonal, FileMode.Create))
                {
                    await RegProfKep.CopyToAsync(stream);
                }
                user.ProfKepUtovnal = $"/images/profKepek/{ujFajlNev}";
                await _userManager.UpdateAsync(user);
            }
            

            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToPage("/FelhOldalak/Fomenu");
        }
        public void OnGet()
        {
        }
    }
}
