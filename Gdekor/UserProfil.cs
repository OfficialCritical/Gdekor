using Microsoft.AspNetCore.Identity;

namespace Gdekor
{
    public class UserProfil:IdentityUser
    {

        public string? SzemelyiSzam { get; set; }
        public string? Nev { get; set; }
        public string? Tel_Korzet {  get; set; }
        public string? SzulHely { get; set; }
        public string? SzulIdo { get; set; }
        public string? AnyjaNeve { get; set; }
        public string? ProfKepUtovnal { get; set; }
    }
}
