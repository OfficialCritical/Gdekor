using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gdekor
{
    public class AppDbContext: IdentityDbContext<UserProfil>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<Projekt> Projektek_Tbl { get; set; }
        public DbSet<Resztvevo_Projektben> ResztvevokProBen_Tbl { get; set; }
        public DbSet<Munkanap> Munkanaptok_Tbl { get; set; }
        public DbSet<Munka_Ora> Munkaorak_Tbl { get; set; }
        public DbSet<Munka_Szunet> MunkaSzunetek_Tbl { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Projekt>(e =>
            {
                e.HasKey(p=>p.Pro_ID);
            });

            builder.Entity<Resztvevo_Projektben>(e =>
            {
                e.HasKey(r => r.ID);

                e.HasOne<Projekt>()
                    .WithMany()
                    .HasForeignKey(r => r.Pro_ID)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne<UserProfil>()
                    .WithMany()
                    .HasForeignKey(r => r.User_ID)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasIndex(r => r.Pro_ID);
            });

            builder.Entity<Munkanap>(e =>
            {
                e.HasKey(m => m.MNap_ID);

                e.HasOne<Projekt>()
                    .WithMany()
                    .HasForeignKey(m => m.Projekt_ID)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne<UserProfil>()
                    .WithMany()
                    .HasForeignKey(m => m.Prof_ID)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasMany<Munka_Ora>()
                    .WithOne()
                    .HasForeignKey(mo => mo.M_Nap_ID)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasMany<Munka_Szunet>()
                    .WithOne()
                    .HasForeignKey(msz => msz.M_Nap_ID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Munka_Ora>(e =>
            {
                e.HasKey(o => o.MOra_ID);

                e.HasIndex(o => o.MOra_ID);
            });

            builder.Entity<Munka_Szunet>(e =>
            {
                e.HasKey(msz => msz.MSzunet_ID);

                e.HasIndex(msz => msz.MSzunet_ID);
            });
        }

        

        
    }
}
