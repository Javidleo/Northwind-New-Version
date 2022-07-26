using Identity.Services;
using Identity.Services.Contracts;
using Identity.ViewModels;
using KnowledgeManagementAPI.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KnowledgeManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly AuthService AuthenticationService;
        private readonly IAppSignInManager SignInManager;
        public AuthController(AuthService authenticationService, IAppSignInManager signInManager)
        {
            AuthenticationService = authenticationService;
            SignInManager = signInManager;
        }

        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (loginDto is null)
                return BadRequest("username and password are not set.");

            var user = await AuthenticationService.FindUserbyUserName(loginDto.UserName);
            if (user == null)
                return BadRequest("Username or password is not correct");
            else
            {
                if (!user.IsActive)
                    return BadRequest("User is not active.");

                var SignInresult = await SignInManager.PasswordSignInAsync(user, loginDto.Password, false, true);
                if (SignInresult.Succeeded)
                {
                    var result = await AuthenticationService.GenerateAccessToken(user, Request);

                    if (result != null)
                        return Ok(new TokenViewModel { AccessToken = result.AccessToken, RefreshToken = result.RefreshToken });
                    else
                        return NoContent();
                }
                if (SignInresult.IsLockedOut)
                {
                    return BadRequest("User is locked.");
                }

                if (SignInresult.IsNotAllowed)
                {
                    return BadRequest("User is not allowed");
                }

            }

            return BadRequest("Username or password is not correct");

        }

        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenDto tokenDto)
        {
            if (tokenDto is null)
                return BadRequest("refreshToken is not set.");

            var refreshTokenValue = tokenDto.RefreshToken;
            if (string.IsNullOrWhiteSpace(refreshTokenValue))
            {
                return BadRequest("refreshToken is not set.");
            }

            var response = await AuthenticationService.RefreshToken(tokenDto.UserId, refreshTokenValue, Request);
            if (response != null)
                return Ok(new TokenViewModel { AccessToken = response.AccessToken, RefreshToken = response.RefreshToken });
            return Unauthorized();

        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<IActionResult> Logout(string refreshToken)
        {
            await AuthenticationService.Logout(refreshToken);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> KillUserSessions([FromBody] KillUserSessionsDto userSessionsDto)
        {
            if (userSessionsDto is null)
                return BadRequest("user is not set.");
            await AuthenticationService.KillUserSessions(userSessionsDto.userGuid);
            return Ok();
        }

        [HttpGet("[action]")]
        //, HttpPost("[action]")]
        public IActionResult IsAuthenticated()
        {
            return Ok(User.Identity.IsAuthenticated);
        }

        [Authorize]
        [HttpGet("[action]")]
        public IActionResult GetUserInfo()
        {
            //ClaimsPrincipal currentUser = this.User;
            //return Ok(currentUser.Identity.Name);

            return Ok(AuthenticationService.GetUserInfo());
            //var claimsIdentity = this.User.Identity as ClaimsIdentity;
            //string FullName = claimsIdentity.FindFirst(ClaimTypes.GivenName).Value;
            //return Ok(new {  claimsIdentity.Name,  FullName });

        }

        //[HttpPost("google")]
        //[ProducesDefaultResponseType]
        //public async Task<IActionResult> GoogleLogin(GoogleLoginRequestDto request)
        //{
        //   // Payload payload;
        //    //try
        //    //{
        //    Payload payload = await ValidateAsync(request.IdToken, new ValidationSettings
        //        {
        //            Audience = new[] { "582817713713-a81f3u4pgecgqf13fhckddspab6aq97b.apps.googleusercontent.com" }                   
        //        });
        //        // It is important to add your ClientId as an audience in order to make sure
        //        // that the token is for your application!
        //    //}
        //    //catch
        //    //{
        //    //    // Invalid token
        //    //}

        //    var user = await AuthenticationService.GetOrCreateExternalLoginUser("google", payload.Subject, payload.Email, payload.GivenName, payload.FamilyName);
        //    if(user!=null)
        //    {
        //        var result = await AuthenticationService.GenerateAccessToken(user, Request);
        //        if (result != null)
        //            return Ok(new TokenViewModel { AccessToken = result.AccessToken, RefreshToken = result.RefreshToken });
        //        else
        //            return NoContent();
        //    }

        //    return BadRequest();

        //}

        //[HttpGet]
        //[Route("google-login")]
        //public IActionResult GoogleLogin()
        //{
        //    var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
        //    return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        //}

        //[HttpGet]
        //[Route("google-response")]
        //public async Task<IActionResult> GoogleResponse()
        //{
        //    var result = await HttpContext.AuthenticateAsync();

        //    var claims = result.Principal.Identities
        //        .FirstOrDefault().Claims.Select(claim => new
        //        {
        //            claim.Issuer,
        //            claim.OriginalIssuer,
        //            claim.Type,
        //            claim.Value
        //        });

        //    return Ok(claims);
        //}
    }
}
