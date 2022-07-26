using Identity.DataSource;
using Identity.Models;
using Identity.Services.Contracts;
using Identity.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Identity.Services
{
    public class AppUserManager : UserManager<AppUser>, IAppUserManager
    {
        private readonly IUserStore<AppUser> _store;
        private readonly IOptions<IdentityOptions> _optionsAccessor;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        private readonly IEnumerable<IUserValidator<AppUser>> _userValidators;
        private readonly IEnumerable<IPasswordValidator<AppUser>> _passwordValidators;
        private readonly ILookupNormalizer _keyNormalizer;
        private readonly IdentityErrorDescriber _errors;
        private readonly IServiceProvider _service;
        private readonly ILogger<UserManager<AppUser>> _logger;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<AppUser> _users;
        private readonly DbSet<AppRole> _roles;
        private readonly DbSet<AppUserRole> _userRoles;
        private readonly IHttpContextAccessor _contextAccessor;

        public AppUserManager(IUserStore<AppUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<AppUser> passwordHasher,
            IEnumerable<IUserValidator<AppUser>> userValidators, IEnumerable<IPasswordValidator<AppUser>> passwordValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider service, ILogger<UserManager<AppUser>> logger, IUnitOfWork uow,
            IHttpContextAccessor contextAccessor)
            : base((UserStore<AppUser, AppRole, ApplicationDbContext, string, AppUserClaim, AppUserRole, AppUserLogin, AppUserToken, AppRoleClaim>)store,
                  optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, service, logger)
        {
            _store = store;
            _optionsAccessor = optionsAccessor;
            _passwordHasher = passwordHasher;
            _userValidators = userValidators;
            _passwordValidators = passwordValidators;
            _keyNormalizer = keyNormalizer;
            _errors = errors;
            _service = service;
            _logger = logger;
            _uow = uow;
            _users = uow.Set<AppUser>();
            _roles = uow.Set<AppRole>();
            _userRoles = uow.Set<AppUserRole>();
            _contextAccessor = contextAccessor;
        }

        public async Task<IdentityResult> AddUserRole(string userId, string roleId, DateTime fromDate, DateTime? toDate, string fromTime, string toTime)
        {
            var appUserRole = new AppUserRole() { UserId = userId, RoleId = roleId, FromDate = fromDate, ToDate = toDate, FromTime = fromTime, ToTime = toTime };
            try
            {
                await _userRoles.AddAsync(appUserRole);
                await _uow.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch
            {
                return IdentityResult.Failed();
            }
        }

        public async Task<IdentityResult> AddUserRole(string userId, string roleId, DateTime fromDate, DateTime? toDate)
        {
            var appUserRole = new AppUserRole() { UserId = userId, RoleId = roleId, FromDate = fromDate, ToDate = toDate };
            try
            {
                await _userRoles.AddAsync(appUserRole);
                await _uow.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch
            {
                return IdentityResult.Failed();
            }
        }

        public async Task<AppUser> FindByPhoneNumberAsync(string phoneNumber, bool verified)
        => await _users.FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber && c.PhoneNumberConfirmed == verified);

        public async Task<AppUser> FindByNationalCodeAsync(string userId, string nationalCode)
        => await _users.FirstOrDefaultAsync(c => c.Id != userId && c.NationalCode == nationalCode);

        public async Task<AppUser> FindByUserIdentityAsync(string userId, string userIdentity)
        => await _users.FirstOrDefaultAsync(c => c.Id != userId && c.UserIdentity == userIdentity);

        public async Task<AppUserRole> FindUserRoleAsync(string userId, string roleId, DateTime fromDate)
        => await _userRoles.FirstOrDefaultAsync(c => c.UserId == userId & c.RoleId == roleId && c.FromDate == fromDate);


        public async Task<AppUserRole> FindUserRoleAsync(string userId, string roleId, DateTime fromDate, string fromTime)
        => await _userRoles.FirstOrDefaultAsync(c => c.UserId == userId & c.RoleId == roleId && c.FromDate == fromDate && c.FromTime == fromTime);


        public async Task<IList<UserRolesViewModel>> GetUserRolesAsync(string userId)
        {
            var query = from role in _roles
                        from userRole in _userRoles
                        where role.Id == userRole.RoleId & userRole.UserId == userId
                        select new UserRolesViewModel
                        {
                            RoleId = role.Id,
                            Name = role.Name,
                            Description = role.Description,
                            IsActive = role.IsActive,
                            FromDate = userRole.FromDate,
                            ToDate = userRole.ToDate,
                            PersianFromDate = NEGSO.Utilities.DateManager.ToPersianDate(userRole.FromDate),
                            PersianToDate = userRole.ToDate != null ? NEGSO.Utilities.DateManager.ToPersianDate(userRole.ToDate.Value) : string.Empty
                        };
            var result = await query.ToListAsync();
            return result;
        }

        public async Task<IdentityResult> SetRoleToDate(AppUserRole userRole, DateTime toDate)
        {
            try
            {
                userRole.ToDate = toDate;
                await _uow.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch
            {
                return IdentityResult.Failed();
            }

        }

        public async Task<IdentityResult> SetRoleToDate(AppUserRole userRole, DateTime toDate, string toTime)
        {
            try
            {
                userRole.ToDate = toDate;
                userRole.ToTime = toTime;
                // _userRoles.Update(userRole);
                await _uow.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch
            {
                return IdentityResult.Failed();
            }

        }

        public AppUser FindById(string userId)
        {
            return _users.Find(userId);
        }

        public string GetCurrentUserId()
        {
            var claimsIdentity = _contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            return claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        public AppUser GetCurrentUser()
        {
            var currentUserId = GetCurrentUserId();
            if (string.IsNullOrWhiteSpace(currentUserId))
                return null;
            return FindById(currentUserId);
        }
        public async Task<AppUser> GetCurrentUserAsync()
        {
            return await GetUserAsync(_contextAccessor.HttpContext.User);
        }
        public async Task<IdentityResult> SetNationalCodeAsync(AppUser user, string nationalCode)
        {
            user.NationalCode = nationalCode;
            var result = await UpdateAsync(user);
            if (!result.Succeeded)
            {
                return result;
            }
            return await SetUserNameAsync(user, nationalCode);
            //if (!result.Succeeded)
            //{
            //    return result;
            //}
            //return await UpdateSecurityStampAsync(user);
        }

        public async Task<IdentityResult> SetActivationStatusAsync(AppUser user, bool status)
        {
            user.IsActive = status;
            var result = await UpdateAsync(user);
            return result;
        }

        Task<IdentityResult> IAppUserManager.UpdatePasswordHash(AppUser user, string newPassword, bool validatePassword)
        {
            return base.UpdatePasswordHash(user, newPassword, validatePassword);
        }

    }
}
