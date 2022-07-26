using Identity.Common;
using Identity.DataSource;
using Identity.Models;
using Identity.Services.Contracts;
using Identity.Settings;
using Identity.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.Services
{
    public class AuthService
    {
        private readonly IAppUserManager UserManager;
        private readonly TokenFactoryService TokenFactoryService;
        private readonly TokenStoreService TokenStoreService;
        private readonly IUnitOfWork UnitOfWork;
        private readonly IHttpContextAccessor ContextAccessor;
        private readonly IOptionsSnapshot<SiteSettings> LoginOptions;
        private readonly IAppSignInManager SignInManager;

        public AuthService(
            IAppUserManager userManager,
            IConfiguration configuration,
            //   UserRepository users,
            TokenFactoryService tokenFactoryService,
            TokenStoreService tokenStoreService,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            IOptionsSnapshot<SiteSettings> loginOptions,
            IAppSignInManager signInManager)
        {
            UserManager = userManager;
            TokenFactoryService = tokenFactoryService;
            TokenStoreService = tokenStoreService;
            UnitOfWork = unitOfWork;
            ContextAccessor = contextAccessor;
            LoginOptions = loginOptions;
            SignInManager = signInManager;
        }
        public async Task<IdentityResult> AuthenticateUser(string username, string password, HttpRequest request)
        {
            var loginOption = LoginOptions.Value.LoginOptions;

            AppUser user = null;
            if (loginOption.LoginWithUserName)
                user = await UserManager.FindByNameAsync(username);
            else if (RegexUtilities.IsValidPhoneNumber(username))
                user = await UserManager.FindByPhoneNumberAsync(username);
            else if (RegexUtilities.IsValidEmail(username))
                user = await UserManager.FindByEmailAsync(username);

            if (user != null)
            {
                var SignInresult = await SignInManager.PasswordSignInAsync(user, password, false, true);
                //  var verigyResult = UserManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
                //   if (user != null && await UserManager.CheckPasswordAsync(user, loginDto.Password))
                // if (verigyResult != PasswordVerificationResult.Failed)
                if (SignInresult.Succeeded)
                {
                    //var userInfo = Users.GetUser(user.Id);
                    //if (userInfo == null)
                    //    throw new KeyNotFoundException("User not Found!!");

                    //string CurrentSource = RequestUtility.GetDeviceInfo(request);

                    //var result = await TokenFactoryService.CreateJwtTokensAsync(user);
                    //await TokenStoreService.AddUserTokenAsync(user, result.RefreshTokenSerial, result.AccessToken, CurrentSource);
                    //await UnitOfWork.SaveChangesAsync();

                    ///// ///set claims for user object of httpcontext
                    //var httpContext = ContextAccessor.HttpContext;
                    //httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(result.Claims, JwtBearerDefaults.AuthenticationScheme));

                    // return result;
                    return IdentityResult.Success;
                }
                if (SignInresult.IsLockedOut)
                {
                    var error1 = new IdentityError
                    {
                        Description = "User is locked."
                    };
                    return IdentityResult.Failed(error1);
                }

                if (SignInresult.IsNotAllowed)
                {
                    var error2 = new IdentityError
                    {
                        Description = "User is not allowed"
                    };
                    return IdentityResult.Failed(error2);
                }

            }

            var error = new IdentityError
            {
                Description = "Username or Password is not Valid"
            };
            return IdentityResult.Failed(error);
        }
        public async Task<JwtTokensData> GenerateAccessToken(AppUser user, HttpRequest request)
        {
            string CurrentSource = RequestUtility.GetDeviceInfo(request);

            var result = await TokenFactoryService.CreateJwtTokensAsync(user);
            if (result != null)
            {
                await TokenStoreService.AddUserTokenAsync(user, result.RefreshTokenSerial, result.AccessToken, CurrentSource);
                await UnitOfWork.SaveChangesAsync();

                /// ///set claims for user object of httpcontext
                var httpContext = ContextAccessor.HttpContext;
                httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(result.Claims, JwtBearerDefaults.AuthenticationScheme));
            }
            return result;
        }
        public async Task<JwtTokensData> RefreshToken(string userId, string refreshToken, HttpRequest request)
        {
            var token = await TokenStoreService.FindTokenbyRefreshTokenAsync(refreshToken);
            if (token == null)
                throw new SecurityTokenValidationException();

            string CurrentSource = RequestUtility.GetDeviceInfo(request);
            if (!TokenStoreService.IsValidTokenSource(token, CurrentSource))
                throw new SecurityTokenValidationException();

            var userInfo = await UserManager.FindByIdAsync(userId);
            // var userInfo = await UserManager.GetUserAsync(request.HttpContext.User);
            if (userInfo == null)
                throw new KeyNotFoundException("User not Found!!");

            var result = await TokenFactoryService.CreateJwtTokensAsync(userInfo);
            await TokenStoreService.AddUserTokenAsync(userInfo, result.RefreshTokenSerial, result.AccessToken, CurrentSource);
            await UnitOfWork.SaveChangesAsync();

            ///set claims for user object of httpcontext
            var httpContext = ContextAccessor.HttpContext;
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(result.Claims, JwtBearerDefaults.AuthenticationScheme));

            return result;
        }
        public UserViewModel GetUserInfo()
        {
            try
            {
                var claimsIdentity = ContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                string FullName = claimsIdentity.FindFirst(ClaimTypes.GivenName).Value;
                var userIdValue = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return new UserViewModel { Username = claimsIdentity.Name, FullName = FullName, UserId = userIdValue };
            }
            catch { throw new BadHttpRequestException("Request is not Valid!!"); }

        }
        public async Task Logout(string refreshToken)
        {
            var claimsIdentity = ContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userIdValue = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await TokenStoreService.RevokeUserBearerTokensAsync(userIdValue, refreshToken);
            await UnitOfWork.SaveChangesAsync();

            //_antiforgery.DeleteAntiForgeryCookies();
        }
        public async Task KillUserSessions(string userGuid)
        {
            await TokenStoreService.InvalidateUserTokensAsync(userGuid);
            await UnitOfWork.SaveChangesAsync();

            //_antiforgery.DeleteAntiForgeryCookies();
        }

        public async Task<AppUser> FindUserbyUserName(string username)
        {
            var loginOption = LoginOptions.Value.LoginOptions;

            AppUser user = null;
            if (loginOption.LoginWithUserName && RegexUtilities.IsValidNationalCode(username))
                user = await UserManager.FindByNameAsync(username);
            if (RegexUtilities.IsValidPhoneNumber(username))
                user = await UserManager.FindByPhoneNumberAsync(username);
            if (RegexUtilities.IsValidEmail(username))
                user = await UserManager.FindByEmailAsync(username);
            return user;
        }

        public async Task<AppUser> GetOrCreateExternalLoginUser(string provider, string key, string email, string firstName, string lastName)
        {
            var user = await UserManager.FindByLoginAsync(provider, key);
            if (user != null)
                return user;

            user = await UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                // No user exists with this email address, we create a new one
                user = new AppUser
                {
                    Email = email,
                    UserName = email,
                    FirstName = firstName,
                    LastName = lastName
                };

                await UserManager.CreateAsync(user);
            }

            var info = new UserLoginInfo(provider, key, provider.ToUpperInvariant());
            var result = await UserManager.AddLoginAsync(user, info);
            if (result.Succeeded)
                return user;
            return null;
        }

    }
}
