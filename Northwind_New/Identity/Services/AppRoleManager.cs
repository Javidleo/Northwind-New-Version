using Identity.DataSource;
using Identity.Models;
using Identity.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;


namespace Identity.Services
{
    public class AppRoleManager : RoleManager<AppRole>, IAppRoleManager
    {
        private readonly IAppRoleStore _store;
        private readonly IEnumerable<IRoleValidator<AppRole>> _roleValidators;
        private readonly ILookupNormalizer _keyNormalizer;
        private readonly IdentityErrorDescriber _errors;
        private readonly ILogger<AppRoleManager> _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<AppUser> _users;
        private readonly DbSet<AppRole> _roles;
        private readonly DbSet<AppUserRole> _userRoles;
        public AppRoleManager(IAppRoleStore store, IEnumerable<IRoleValidator<AppRole>> roleValidators, ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors, ILogger<AppRoleManager> logger, IHttpContextAccessor contextAccessor, IUnitOfWork uow)
            : base((RoleStore<AppRole, ApplicationDbContext, string, AppUserRole, AppRoleClaim>)store, roleValidators, keyNormalizer, errors, logger)
        {
            _store = store;
            _roleValidators = roleValidators;
            _keyNormalizer = keyNormalizer;
            _errors = errors;
            _logger = logger;
            _contextAccessor = contextAccessor;
            _uow = uow;
            _users = uow.Set<AppUser>();
            _roles = uow.Set<AppRole>();
            _userRoles = uow.Set<AppUserRole>();
        }

        private string GetCurrentUserId()
        {
            var claimsIdentity = _contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            return claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        public bool IsCurrentUserInRole(string roleName)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return false;
            return IsUserInRole(userId, roleName);
        }

        public bool IsUserInRole(string userId, string roleName)
        {
            var userRolesQuery = from role in _roles
                                 where role.Name == roleName
                                 from user in role.Users
                                 where user.UserId == userId
                                 select role;
            var userRole = userRolesQuery.FirstOrDefault();
            return userRole != null;
        }


    }
}
