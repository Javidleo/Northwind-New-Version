using Identity.DataSource.Mapping;
using Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Identity.DataSource
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, string, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>, IUnitOfWork
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new AppUserMapping());
            builder.ApplyConfiguration(new AppUserRoleMapping());
            builder.ApplyConfiguration(new UserJWTTokenMapping());
            builder.ApplyConfiguration(new UserSmsTokenMapping());

        }

        public DbSet<UserJWTToken> UserJWTTokens { get; set; }
        public DbSet<UserSmsToken> UserSmsTokens { get; set; }
    }
}
