using Identity.Models;
using Identity.Services.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace Identity.Services
{
    public class AppSignInManager : SignInManager<AppUser>, IAppSignInManager
    {
        private readonly IAppUserManager _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUserClaimsPrincipalFactory<AppUser> _claimsFactory;
        private readonly IOptions<IdentityOptions> _optionsAccessor;
        private readonly ILogger<AppSignInManager> _logger;
        private readonly IAuthenticationSchemeProvider _schemes;
        private readonly IUserConfirmation<AppUser> _confirmaton;
        public AppSignInManager(IAppUserManager userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<AppUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<AppSignInManager> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<AppUser> confirmaton)
            : base((UserManager<AppUser>)userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmaton)
        {
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _claimsFactory = claimsFactory;
            _optionsAccessor = optionsAccessor;
            _logger = logger;
            _schemes = schemes;
            _confirmaton = confirmaton;
        }
    }
}
