using Identity.DataSource;
using Identity.Models;
using Identity.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Identity.Services
{
    public class AppRoleStore : RoleStore<AppRole, ApplicationDbContext, string, AppUserRole, AppRoleClaim>, IAppRoleStore
    {
        public AppRoleStore(IUnitOfWork unitOfWork, IdentityErrorDescriber errorDescriber = null)
            : base((ApplicationDbContext)unitOfWork, errorDescriber)
        { }

    }
}
