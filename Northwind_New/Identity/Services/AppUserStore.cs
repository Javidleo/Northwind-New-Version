using Identity.DataSource;
using Identity.Models;
using Identity.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Identity.Services
{
    public class AppUserStore : UserStore<AppUser, AppRole, ApplicationDbContext, string, AppUserClaim, AppUserRole, AppUserLogin, AppUserToken, AppRoleClaim>, IAppUserStore
    {
        public AppUserStore(IUnitOfWork unitOfWork, IdentityErrorDescriber describer = null) : base((ApplicationDbContext)unitOfWork, describer)
        { }
    }
}
