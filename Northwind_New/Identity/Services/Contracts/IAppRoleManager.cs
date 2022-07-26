using Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Services.Contracts
{
    public interface IAppRoleManager
    {

        #region BaseClass

        IQueryable<AppRole> Roles { get; }
        Task<AppRole> FindByIdAsync(string roleId);

        Task<AppRole> FindByNameAsync(string name);
        Task<IdentityResult> CreateAsync(AppRole appRole);
        Task<IdentityResult> UpdateAsync(AppRole role);
        #endregion

        #region CustomMethods
        bool IsCurrentUserInRole(string roleName);
        #endregion
    }
}