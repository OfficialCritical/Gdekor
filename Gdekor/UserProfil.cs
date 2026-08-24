using Microsoft.AspNetCore.Identity;

namespace Gdekor
{
    public class UserProfil:IdentityUser
    {

        public string? SzemelyiSzam { get; set; } 
        public string? LakcimK_Szama { get; set; } 
        public string? Adoszam { get; set; } 
        public string? Tajszam { get; set; } 
        public string? Nev { get; set; }
        public string? Tel_Korzet {  get; set; }
        public string? Lakcim { get; set; }
        public string? SzulHely { get; set; }
        public string? SzulIdo { get; set; }
        public string? AnyjaNeve { get; set; }
        public string? ProfKepUtovnal { get; set; }

        public string? JogsiSzama { get; set; }
        public string? JogsiKateg { get; set; }
        public string? Rendszam { get; set; }
        public string? Ferohely { get; set; }

    }
}
