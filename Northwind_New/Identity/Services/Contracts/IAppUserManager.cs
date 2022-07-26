using Identity.Models;
using Identity.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Identity.Services.Contracts
{
    public interface IAppUserManager
    {

        #region BaseClass
        IPasswordHasher<AppUser> PasswordHasher { get; set; }
        Task<AppUser> FindByIdAsync(string userId);
        Task<AppUser> FindByEmailAsync(string email);
        Task<AppUser> FindByLoginAsync(string loginProvider, string providerKey);
        Task<IdentityResult> AddLoginAsync(AppUser user, UserLoginInfo login);
        Task<IdentityResult> CreateAsync(AppUser user, string password);
        Task<IdentityResult> CreateAsync(AppUser user);
        Task<IdentityResult> UpdateAsync(AppUser user);
        Task<AppUser> FindByNameAsync(string name);
        Task<IdentityResult> SetLockoutEnabledAsync(AppUser adminUser, bool enabled);
        Task<AppUser> GetUserAsync(ClaimsPrincipal user);
        Task<IdentityResult> SetEmailAsync(AppUser user, string email);
        Task<string> GetSecurityStampAsync(AppUser user);
        Task<IdentityResult> UpdateSecurityStampAsync(AppUser user);
        Task<IdentityResult> SetUserNameAsync(AppUser user, string userName);
        Task<IdentityResult> ChangePasswordAsync(AppUser user, string currentPassword, string newPassword);
        Task<IdentityResult> UpdatePasswordHash(AppUser user, string newPassword, bool validatePassword);
        Task<IList<String>> GetRolesAsync(AppUser user);
        Task<string> GenerateChangePhoneNumberTokenAsync(AppUser user, string phoneNumber);
        Task<bool> VerifyChangePhoneNumberTokenAsync(AppUser user, string token, string phoneNumber);
        Task<IdentityResult> ChangePhoneNumberAsync(AppUser user, string phoneNumber, string token);
        Task<bool> IsPhoneNumberConfirmedAsync(AppUser user);
        Task<string> GenerateEmailConfirmationTokenAsync(AppUser user);
        Task<IdentityResult> ConfirmEmailAsync(AppUser user, string token);
        Task<bool> IsEmailConfirmedAsync(AppUser user);
        Task<string> GenerateChangeEmailTokenAsync(AppUser user, string newEmail);
        Task<IdentityResult> ChangeEmailAsync(AppUser user, string newEmail, string token);




        #endregion

        #region CustomMethods
        AppUser FindById(string userId);
        Task<AppUser> FindByPhoneNumberAsync(string phoneNumber, bool verified = true);
        Task<AppUser> FindByNationalCodeAsync(string userId, string nationalCode);
        Task<AppUser> FindByUserIdentityAsync(string userId, string userIdentity);
        Task<IdentityResult> AddUserRole(string userId, string roleId, DateTime fromDate, DateTime? toDate);
        Task<IdentityResult> AddUserRole(string userId, string roleId, DateTime fromDate, DateTime? toDate, string fromTime, string toTime);

        Task<AppUserRole> FindUserRoleAsync(string userId, string roleId, DateTime fromDate);
        Task<AppUserRole> FindUserRoleAsync(string userId, string roleId, DateTime fromDate, string fromTime);

        Task<IdentityResult> SetRoleToDate(AppUserRole userRole, DateTime toDate);
        Task<IdentityResult> SetRoleToDate(AppUserRole userRole, DateTime toDate, string toTime);
        Task<IList<UserRolesViewModel>> GetUserRolesAsync(string userId);
        string GetCurrentUserId();
        AppUser GetCurrentUser();
        Task<AppUser> GetCurrentUserAsync();
        Task<IdentityResult> SetNationalCodeAsync(AppUser user, string nationalCode);
        Task<IdentityResult> SetActivationStatusAsync(AppUser user, bool status);

        #endregion


    }
}