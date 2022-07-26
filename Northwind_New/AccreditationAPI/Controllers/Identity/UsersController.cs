using Identity.Common;
using Identity.Services;
using Identity.Services.Contracts;
using Identity.Settings;
using KnowledgeManagementAPI.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;


namespace KnowledgeManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        readonly IAppUserManager UserManager;
        readonly UserService UserService;
        private readonly IOptionsSnapshot<SiteSettings> _siteSettings;
        public UsersController(IAppUserManager userManager, UserService userService, IOptionsSnapshot<SiteSettings> siteSettings)
        {
            UserManager = userManager;
            UserService = userService;
            _siteSettings = siteSettings;
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (registerDto is null)
                return BadRequest("Parameters are not set.");

            var result = await UserService.RegisterUser(registerDto.PhoneNumber, registerDto.Password);
            if (result.Item1.Succeeded)
            {
                var smsLifeSpan = _siteSettings.Value.SmsConfirmationLifespan;
                return CreatedAtAction("Register", new { smsLifeSpan = smsLifeSpan, code = result.Item2 });
            }
            return BadRequest(result.Item1.DumpErrors());

            // return Ok(new { UserId = user.Id });
            // return Ok($"User Registered With UserName {user.UserName}");

        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> ResendSmsToken([FromBody] SendSmsTokenDto smsTokenDto)
        {
            if (smsTokenDto is null)
                return BadRequest("Parameters are not set.");

            var user = await UserManager.FindByPhoneNumberAsync(smsTokenDto.phoneNumber, false);
            if (user == null)
                return BadRequest("PhoneNumber is not valid.");

            var token = await UserManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
            if (token != null)
            {
                await UserService.AddSmsToken(user, token, "Resend token in User Registeration.");

                ///To Do
                ///Send token to User's PhoneNumber
                ///
                var smsLifeSpan = _siteSettings.Value.SmsConfirmationLifespan;
                return Ok(new { smsLifeSpan = smsLifeSpan, code = token });
            }



            return BadRequest();
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyPhoneNumber([FromBody] MobileVerificationDto smsToken)
        {
            if (smsToken is null)
                return BadRequest("Parameters are not set.");

            var result = await UserService.VerifyPhoneNumber(smsToken.PhoneNumber, smsToken.Code);
            if (result.Succeeded)
                return Ok();
            return BadRequest(result.DumpErrors());
        }

        //[HttpPost("[action]")]
        //[AllowAnonymous]
        //public async Task<IActionResult> VerifyUser([FromBody] UserVerificationDto verificationDto)
        //{
        //    if (verificationDto is null)
        //        return BadRequest("Parameters are not set.");

        //    var result = await UserService.VerifyUser(verificationDto.UserName,verificationDto.Code);
        //    if (result.Succeeded)
        //        return Ok();
        //    return BadRequest(result.DumpErrors());
        //}


        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole([FromBody] UserRoleDto userRoleDto)
        {
            if (userRoleDto is null)
                return BadRequest("Parameters are not set.");

            var result = await UserService.AddUserRole(userRoleDto.UserId, userRoleDto.RoleId, userRoleDto.FromDate, userRoleDto.ToDate, userRoleDto.FromTime, userRoleDto.TOTime);
            if (result.Succeeded)
                return Ok();
            return BadRequest(result.DumpErrors());
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnAssignRole([FromBody] UserRoleDto userRoleDto)
        {
            if (userRoleDto is null)
                return BadRequest("Parameters are not set.");

            var result = await UserService.SetRoleFinishDate(userRoleDto.UserId, userRoleDto.RoleId, userRoleDto.FromDate, userRoleDto.ToDate, userRoleDto.FromTime, userRoleDto.TOTime);
            if (result.Succeeded)
                return Ok();
            return BadRequest(result.DumpErrors());
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("Parameters are not set.");

            var result = await UserService.GetUserRoles(userId);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SetEmail([FromBody] AddEmailDto emailDto)
        {
            if (emailDto is null)
                return BadRequest("Parameters are not set.");

            /// To Do
            /// Send Verficaton Link To EmailAddress
            /// 
            ///when email is verified then continue or add email for user but  field 'EmailConfirmed' will be false ?????



            var result = await UserService.SetUserEmail(emailDto.UserId, emailDto.Email, Request);
            if (result.Succeeded)
                return Ok();
            return BadRequest(result.DumpErrors());
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SetProfile([FromBody] UserProfileDto profileDto)
        {
            if (profileDto is null)
                return BadRequest("Parameters are not set.");

            var result = await UserService.SetUserProfile(profileDto.UserId, profileDto.UserIdentity, profileDto.FirstName, profileDto.LastName, profileDto.BirthDate, profileDto.Gender);
            if (result.Succeeded)
                return Ok();
            return BadRequest(result.DumpErrors());
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SetNationalCode([FromBody] AddNationalCodeDto nationalCodeDto)
        {
            if (nationalCodeDto is null)
                return BadRequest("Parameters are not set.");

            var result = await UserService.SetUserNationalCode(nationalCodeDto.UserId, nationalCodeDto.NationalCode);
            if (result.Succeeded)
                return Ok();
            return BadRequest(result.DumpErrors());
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            if (changePasswordDto is null)
                return BadRequest("Parameters are not set.");
            var result = await UserService.ChangePassword(changePasswordDto.OldPassword, changePasswordDto.NewPassword);
            if (result.Succeeded)
                return Ok();
            return BadRequest(result.DumpErrors());
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ChangeUserPassword([FromBody] ChangeUserPasswordDto changeUserPasswordDto)
        {
            if (changeUserPasswordDto is null)
                return BadRequest("Parameters are not set.");

            var result = await UserService.ChangeUserPassword(changeUserPasswordDto.UserId, changeUserPasswordDto.NewPassword);
            if (result.Succeeded)
                return Ok();
            return BadRequest(result.DumpErrors());
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ChangePhoneNumber([FromBody] ChangePhoneNumberDto phoneNumberDto)
        {
            if (phoneNumberDto is null)
                return BadRequest("Parameters are not set.");
            var result = await UserService.ChangePhoneNumber(phoneNumberDto.UserId, phoneNumberDto.PhoneNumber);
            if (result.Succeeded)
                return Ok();
            return BadRequest(result.DumpErrors());

        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SetUserActivationStatus([FromBody] UserActivationStatusDto activationStatusDto)
        {
            if (activationStatusDto is null)
                return BadRequest("Parameters are not set.");

            var result = await UserService.SetUserActivationStatus(activationStatusDto.UserId, activationStatusDto.IsActive);
            if (result.Succeeded)
                return Ok();
            return BadRequest(result.DumpErrors());
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CheckUserIdentity([FromBody] UserIdentityDto userIdentityDto)
        {
            if (userIdentityDto is null)
                return BadRequest("Parameters are not set.");

            var result = await UserService.CheckUserIdentity(userIdentityDto.UserId, userIdentityDto.UserIdentity);
            if (result != null)
                return Conflict(result);
            return Ok();
        }
    }
}
