
using Identity.Services.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.Services
{
    public class TokenValidatorService
    {
        // readonly UserRepository Users;
        readonly IAppUserManager UserManager;
        readonly TokenStoreService TokenStoreService;

        public TokenValidatorService(TokenStoreService tokenStoreService
            , IAppUserManager userManager)   //UserRepository userRepository, 
        {
            //   Users = userRepository;
            UserManager = userManager;
            TokenStoreService = tokenStoreService;
        }
        public async Task ValidateAsync(TokenValidatedContext context)
        {
            var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
            if (claimsIdentity?.Claims == null || !claimsIdentity.Claims.Any())
            {
                context.Fail("This is not our issued token. It has no claims.");
                return;
            }

            var serialNumberClaim = claimsIdentity.FindFirst(ClaimTypes.SerialNumber);
            if (serialNumberClaim == null)
            {
                context.Fail("This is not our issued token. It has no serial.");
                return;
            }

            //var userIdString = claimsIdentity.FindFirst(ClaimTypes.UserData).Value;
            //if (!int.TryParse(userIdString, out int userId))
            //{
            //    context.Fail("This is not our issued token. It has no user-id.");
            //    return;
            //}

            //var userId= claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;


            var User = await UserManager.GetUserAsync(context.Principal);

            ///check if User's Data has changed since last login
            string serialNumber = await UserManager.GetSecurityStampAsync(User);
            if (string.IsNullOrWhiteSpace(serialNumber) || serialNumber != serialNumberClaim.Value)
            {
                // user has changed his/her password/roles/stat/IsActive
                context.Fail("This token is expired. Please login again.");
                return;
            }

            //if (!User.lo)
            //{
            //    // user has changed his/her password/roles/stat/IsActive
            //    context.Fail("This token is expired. Please login again.");
            //}

            if (!(context.SecurityToken is JwtSecurityToken accessToken) || string.IsNullOrWhiteSpace(accessToken.RawData) ||
                !await TokenStoreService.IsValidTokenAsync(accessToken.RawData, User.Id))
            {
                context.Fail("This token is not in our database.");
                return;
            }

            //  await _usersService.UpdateUserLastActivityDateAsync(userId);
        }
    }
}
