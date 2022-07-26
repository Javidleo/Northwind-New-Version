using Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;


namespace Identity.Services.Contracts
{
    public interface IAppSignInManager
    {
        #region baseClass
        UserManager<AppUser> UserManager { get; set; }
        Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);
        Task<SignInResult> PasswordSignInAsync(AppUser user, string password, bool isPersistent, bool lockoutOnFailure);
        #endregion
    }
}
