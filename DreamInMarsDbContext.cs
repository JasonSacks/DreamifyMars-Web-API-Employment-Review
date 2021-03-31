using DreamInMars.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace DreamInMars
{
    public class DreamInMarsDbContext : IdentityDbContext<DreamUser>
    {
        public DreamInMarsDbContext(DbContextOptions<DreamInMarsDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<DreamUser>(c => c.ToTable("Users"));
            builder.Entity<IdentityRole>(c => c.ToTable("Roles"));
            builder.Entity<IdentityUserRole<string>>(c => c.ToTable("UserRoles"));
            builder.Entity<IdentityUserClaim<string>>(c => c.ToTable("UserClaims"));
            builder.Entity<IdentityUserLogin<string>>(c => c.ToTable("UserLogins"));
            builder.Entity<IdentityUserToken<string>>(c => c.ToTable("UserTokens"));
            builder.Entity<IdentityRoleClaim<string>>((c => c.ToTable("RoleClaims")));
            builder.Entity<Account>(c => c.ToTable("Accounts"));
            builder.Entity<GalleryImage>(c => c.ToTable("GalleryImages"));
            builder.Entity<Credit>(c => c.ToTable("Credits"));
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<GalleryImage> GalleryImages { get; set; }
        public DbSet<Credit> Credits{ get; set; }
    }
}
