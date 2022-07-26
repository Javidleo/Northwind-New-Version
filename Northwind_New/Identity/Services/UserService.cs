using Identity.Models;
using Identity.Repositories;
using Identity.Services.Contracts;
using Identity.Settings;
using Identity.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;


namespace Identity.Services
{
    public class UserService
    {
        private readonly IAppUserManager UserManager;
        private readonly IAppRoleManager RoleManager;
        private readonly IOptionsSnapshot<SiteSettings> _siteSettings;
        private readonly UserSmsTokenRepository UserSmsTokens;
        private readonly EmailService EmailService;
        private readonly IUrlHelper UrlHelper;

        public UserService(IAppUserManager userManager,
                           IAppRoleManager roleManager,
                           IOptionsSnapshot<SiteSettings> siteSettings,
                           UserSmsTokenRepository userSmsTokens,
                           EmailService emailService,
                           IUrlHelper urlHelper)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            _siteSettings = siteSettings;
            UserSmsTokens = userSmsTokens;
            EmailService = emailService;
            UrlHelper = urlHelper;
        }

        public async Task<IdentityResult> AddUserRole(string userId, string roleId, string fromDate, string toDate, string fromTime, string toTime)
        {
            var UserExists = await UserManager.FindByIdAsync(userId);
            if (UserExists == null)
                throw new KeyNotFoundException("User does not Exist.");

            var roleExists = await RoleManager.FindByIdAsync(roleId);
            if (roleExists == null)
                throw new KeyNotFoundException("Role does not exist.");

            if (string.IsNullOrEmpty(fromDate))
                throw new BadHttpRequestException("FromDate is mandatory.");

            //var _fromDate = fromDate==null ? DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified) : DateTime.SpecifyKind((DateTime)fromDate, DateTimeKind.Unspecified);
            //DateTime? _toDate = toDate !=null ? DateTime.SpecifyKind((DateTime)toDate, DateTimeKind.Unspecified):null;

            DateTime FromDate = DateTime.Parse(fromDate).Date;
            DateTime? ToDate = !string.IsNullOrEmpty(toDate) ? DateTime.Parse(toDate).Date : null;

            var result = await UserManager.AddUserRole(userId, roleId, FromDate, ToDate, fromTime, toTime);

            return result;
        }

        public async Task<IdentityResult> SetRoleFinishDate(string userId, string roleId, string fromDate, string toDate, string fromTime, string toTime)
        {
            var UserExists = await UserManager.FindByIdAsync(userId);
            if (UserExists == null)
                throw new KeyNotFoundException("User does not Exist.");

            var roleExists = await RoleManager.FindByIdAsync(roleId);
            if (roleExists == null)
                throw new KeyNotFoundException("Role does not exist.");


            if (string.IsNullOrEmpty(fromDate))
                throw new BadHttpRequestException("FromDate is mandatory.");

            if (string.IsNullOrEmpty(toDate))
                throw new BadHttpRequestException("ToDate is mandatory.");

            DateTime FromDate = DateTime.Parse(fromDate).Date;
            DateTime ToDate = DateTime.Parse(toDate).Date;

            //   var _fromDate =  DateTime.SpecifyKind((DateTime)fromDate.Value, DateTimeKind.Unspecified);
            //DateTime _toDate =  DateTime.SpecifyKind((DateTime)toDate, DateTimeKind.Unspecified);


            var userRole = await UserManager.FindUserRoleAsync(userId, roleId, FromDate, fromTime);
            if (userRole != null)
            {
                var result = await UserManager.SetRoleToDate(userRole, ToDate, toTime);
                return result;
            }

            return IdentityResult.Failed();
        }

        public async Task<IList<UserRolesViewModel>> GetUserRoles(string userId)
        {
            return await UserManager.GetUserRolesAsync(userId);
        }

        public async Task<IdentityResult> SetUserEmail(string userId, string email, HttpRequest request)  //,
        {
            //if (userId != UserManager.GetCurrentUserId() &&
            //   !RoleManager.IsCurrentUserInRole("Admin"))
            //    throw new AccessViolationException("User does not have permission to change profile.");

            var User = await UserManager.FindByIdAsync(userId);
            if (User == null)
                throw new KeyNotFoundException("User does not Exist.");

            var emailExists = await UserManager.FindByEmailAsync(email);
            if (emailExists != null)
                throw new BadHttpRequestException("EmailAddress already Exists.");

            if (await UserManager.IsEmailConfirmedAsync(User))
            {

                var result = await UserManager.SetEmailAsync(User, email);
                //  return result;
                if (result.Succeeded)
                {
                    var token = await UserManager.GenerateChangeEmailTokenAsync(User, email);

                    /// send token to email
                    ///
                    // string link = new UrlHelper(request.Path).Action("Index", "Home", null, HttpContext.Current.Request.Url.Scheme);

                    var confirmationLink = UrlHelper.Action("VerifyEmail", "Email", new { email = email, confirmToken = token }, request.Scheme);

                    string body = "<br/><br/>We are excited to tell you that your account is" +
"                             successfully created. Please click on the below link to verify your account" +
"                                 <br/><br/><a href='" + confirmationLink + "'>" + confirmationLink + "</a> ";

                    //var confirmationLink = request.Body.Url.Action(nameof(VerifyEmail), "Email", new {  email = email, confirmToken=token }, Request.Scheme);
                    //var message = new Message(new string[] { user.Email }, "Confirmation email link", confirmationLink, null);

                    await EmailService.SendEmail(email, body);
                }

                return result;

                //return await UserManager.UpdateSecurityStampAsync(User);
            }
            else
            {
                var result = await UserManager.SetEmailAsync(User, email);
                if (result.Succeeded)
                {

                    var confirmToken = await UserManager.GenerateEmailConfirmationTokenAsync(User);

                    var confirmationLink = UrlHelper.Action("VerifyEmail", "Email", new { email = email, confirmToken = confirmToken }, request.Scheme);

                    string body = "<br/><br/>We are excited to tell you that your account is" +
"                             successfully created. Please click on the below link to verify your account" +
"                                 <br/><br/><a href='" + confirmationLink + "'>" + confirmationLink + "</a> ";

                    /// send token to email
                    /// 
                    await EmailService.SendEmail(email, body);
                }

                return result;
                //if (!result.Succeeded)
                //{
                //    return result;
                //}
                //return await UserManager.UpdateSecurityStampAsync(User);
            }

            // generate url
            //var confirmationLink = _urlHelper.Action("ConfirmEmail", nameof(UsersController),
            //        new { userId = newUser.Id, token = token }, _httpRequest.HttpContext.Request.Scheme);

            //var confirmationLink = Url.Action("ConfirmEmail",
            //  "Account", new
            //  {
            //      userid = user.Id,
            //      token = confirmationToken
            //  },
            //   protocol: HttpContext.Request.Scheme);



            // return IdentityResult.Success;

        }

        public async Task<IdentityResult> SetUserProfile(string userId, string userIdentity, string name, string lastName, string birthDate, Int16? gender)
        {
            //if (userId != UserManager.GetCurrentUserId() &&
            //    !RoleManager.IsCurrentUserInRole("Admin"))
            //    throw new AccessViolationException("User does not have permission to change profile.");

            var User = await UserManager.FindByIdAsync(userId);
            if (User == null)
                throw new KeyNotFoundException("User does not Exist.");

            var UserIdentityExist = await UserManager.FindByUserIdentityAsync(userId, userIdentity);
            if (UserIdentityExist != null)
                throw new DuplicateNameException("UserIdentity Already Exists.Please try another one.");

            if (!string.IsNullOrWhiteSpace(birthDate))
                User.BirthDate = birthDate;
            if (!string.IsNullOrWhiteSpace(name))
                User.FirstName = name;
            if (!string.IsNullOrWhiteSpace(lastName))
                User.LastName = lastName;
            if (gender != null)
                User.Gender = gender;
            User.UserIdentity = userIdentity;

            var result = await UserManager.UpdateAsync(User);
            if (!result.Succeeded)
            {
                return result;
            }
            return await UserManager.UpdateSecurityStampAsync(User);
        }

        public async Task<IdentityResult> SetUserNationalCode(string userId, string nationalCode)
        {
            //if (userId != UserManager.GetCurrentUserId() &&
            //   !RoleManager.IsCurrentUserInRole("Admin"))
            //    throw new AccessViolationException("User does not have permission to change profile.");

            var User = await UserManager.FindByIdAsync(userId);
            if (User == null)
                throw new KeyNotFoundException("User does not Exist.");

            var codeExists = await UserManager.FindByNationalCodeAsync(userId, nationalCode);
            if (codeExists != null)
                throw new BadHttpRequestException("NationalCode already Exists.");

            var result = await UserManager.SetNationalCodeAsync(User, nationalCode);
            if (!result.Succeeded)
            {
                return result;
            }
            return await UserManager.UpdateSecurityStampAsync(User);
        }

        public async Task<UserIdentitySuggestionList> CheckUserIdentity(string userId, string userIdentity)
        {
            //if (userId != UserManager.GetCurrentUserId())
            //    throw new AccessViolationException("User does not have permission to change profile.");

            var User = await UserManager.FindByIdAsync(userId);
            if (User == null)
                throw new KeyNotFoundException("User does not Exist.");

            UserIdentitySuggestionList userIdentitySuggestionList = null;

            var UserIdentityExist = await UserManager.FindByUserIdentityAsync(userId, userIdentity);
            if (UserIdentityExist != null)
            {
                var phoneNumber = User.PhoneNumber;

                var suggestionList = new List<string>();
                if (await UserManager.FindByUserIdentityAsync(userId, userIdentity + phoneNumber.Substring(7, 4)) == null)
                    suggestionList.Add(userIdentity + phoneNumber.Substring(7, 4));
                if (await UserManager.FindByUserIdentityAsync(userId, userIdentity + phoneNumber.Substring(8, 3)) == null)
                    suggestionList.Add(userIdentity + phoneNumber.Substring(8, 3));
                if (await UserManager.FindByUserIdentityAsync(userId, userIdentity + phoneNumber.Substring(9, 2)) == null)
                    suggestionList.Add(userIdentity + phoneNumber.Substring(9, 2));

                userIdentitySuggestionList = new UserIdentitySuggestionList { SuggestionList = suggestionList };
            }

            return userIdentitySuggestionList;
        }

        public async Task<IdentityResult> SetUserActivationStatus(string userId, bool status)
        {
            var User = await UserManager.FindByIdAsync(userId);
            if (User == null)
                throw new KeyNotFoundException("User does not Exist.");

            var result = await UserManager.SetActivationStatusAsync(User, status);
            if (!result.Succeeded)
            {
                return result;
            }
            return await UserManager.UpdateSecurityStampAsync(User);
        }
        public async Task<IdentityResult> ChangePassword(string oldPassword, string newPassword)
        {
            var user = await UserManager.GetCurrentUserAsync();
            if (user == null)
                throw new KeyNotFoundException("There is no Loged in user.");

            var result = await UserManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!result.Succeeded)
            {
                return result;
            }
            return await UserManager.UpdateSecurityStampAsync(user);
        }
        public async Task<IdentityResult> ChangeUserPassword(string userId, string newPassword)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User does not Exist.");

            var result = await UserManager.UpdatePasswordHash(user, newPassword, true);
            if (!result.Succeeded)
            {
                return result;
            }
            return await UserManager.UpdateSecurityStampAsync(user);
        }
        public async Task AddSmsToken(AppUser user, string token, string description)
        {
            var smsLifeSpan = _siteSettings.Value.SmsConfirmationLifespan;
            var smsToken = new UserSmsToken()
            {
                UserId = user.Id,
                PhoneNumber = user.PhoneNumber,
                Code = token,
                Description = description,
                CreateDate = DateTime.Now,
                ExpireDate = DateTime.Now.AddMinutes(smsLifeSpan)
            };

            await UserSmsTokens.AddAsync(smsToken);
        }

        public async Task<IdentityResult> VerifyPhoneNumber(string userId, string code, string phoneNumber)
        {
            var User = await UserManager.FindByIdAsync(userId);
            if (User == null)
                throw new KeyNotFoundException("User does not Exist.");

            var userSmsToken = UserSmsTokens.GetLastbyUserId(userId);
            if (userSmsToken != null)
            {
                if (userSmsToken.Code == code && userSmsToken.ExpireDate >= DateTime.Now)
                {
                    var response = await UserManager.ChangePhoneNumberAsync(User, User.PhoneNumber, code);
                    return response;
                }
                return IdentityResult.Failed();
            }
            return IdentityResult.Failed();
        }

        public async Task<IdentityResult> VerifyPhoneNumber(string phoneNumber, string code)
        {
            var User = await UserManager.FindByPhoneNumberAsync(phoneNumber, false);
            if (User == null)
                throw new KeyNotFoundException("PhoneNumber is not Valid.");

            var userSmsToken = UserSmsTokens.GetLastbyPhoneNumber(phoneNumber);
            if (userSmsToken != null)
            {
                if (userSmsToken.Code == code && userSmsToken.ExpireDate >= DateTime.Now)
                {
                    var response = await UserManager.ChangePhoneNumberAsync(User, User.PhoneNumber, code);
                    return response;
                }
                return IdentityResult.Failed();
            }
            return IdentityResult.Failed();
        }

        public async Task<IdentityResult> ChangePhoneNumber(string userId, string phoneNumber)
        {
            var User = await UserManager.FindByIdAsync(userId);
            if (User == null)
                throw new KeyNotFoundException("User does not Exist.");
            if (!User.PhoneNumberConfirmed)
            {
                User.PhoneNumber = phoneNumber;
                var result = await UserManager.UpdateAsync(User);
                if (result.Succeeded)
                {
                    var response = await UserManager.SetUserNameAsync(User, phoneNumber);
                    if (response.Succeeded)
                    {
                        var token = await UserManager.GenerateChangePhoneNumberTokenAsync(User, User.PhoneNumber);
                        await AddSmsToken(User, token, "ChangePhoneNumber");

                        ///To Do
                        ///Send token to User's PhoneNumber
                        ///
                    }

                }
                return result;
            }
            throw new BadHttpRequestException("امکان تغییر شماره تلفن وجود ندارد");

        }

        //public async Task<IdentityResult> VerifyUser(string username,string code)
        //{
        //    if (RegexUtilities.IsValidPhoneNumber(username))
        //        return await VerifyPhoneNumber(username,code);

        //    else if (RegexUtilities.IsValidEmail(username))
        //        return await VerifyEmail(username, code);

        //    return IdentityResult.Success;
        //}

        public async Task<(IdentityResult, string)> RegisterUser(string phoneNumber, string password)
        {
            AppUser user = null;

            user = await UserManager.FindByPhoneNumberAsync(phoneNumber);
            if (user != null)
                throw new DuplicateNameException("PhoneNumber Already Exists.");

            user = await UserManager.FindByPhoneNumberAsync(phoneNumber, false);
            if (user != null)
            {
                var changePassResult = await ChangeUserPassword(user.Id, password);
                if (!changePassResult.Succeeded)
                {
                    return (changePassResult, "");
                }

            }
            else
            {
                user = new AppUser()
                {
                    UserName = phoneNumber,
                    PhoneNumber = phoneNumber,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var result = await UserManager.CreateAsync(user, password);
                if (result.Errors.Any())
                    return (result, "");
            }

            var token = await UserManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
            await AddSmsToken(user, token, "User Registeration");

            ///To Do
            ///Send token to User's PhoneNumber
            ///

            string msg = "کد فعالسازی :" + token + "مدیریت دانش- نسل آینده راهکارها";

            return (IdentityResult.Success, token);
        }
    }
}
