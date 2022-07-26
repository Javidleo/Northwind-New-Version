using Identity.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.Services
{
    public class EmailService
    {
        private readonly IAppUserManager UserManager;
        private readonly IEmailSender EmailSender;
        public EmailService(IAppUserManager userManager, IEmailSender emailSender)
        {
            UserManager = userManager;
            EmailSender = emailSender;
        }


        public async Task SendEmail(string email, string message)
        {
            await EmailSender.SendEmailAsync(email, message);
        }
        public async Task<IdentityResult> VerifyEmail(string token)
        {
            var user = await UserManager.GetCurrentUserAsync();
            if (user == null)
                throw new KeyNotFoundException("User is not Valid.");

            var result = await UserManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return result;
            }
            return await UserManager.UpdateSecurityStampAsync(user);
        }

        public async Task<IdentityResult> VerifyEmail(string email, string confirmToken)
        {
            var User = await UserManager.FindByEmailAsync(email);
            if (User == null)
                throw new KeyNotFoundException("Email does not Exist.");

            var result = await UserManager.ConfirmEmailAsync(User, confirmToken);
            if (!result.Succeeded)
            {
                return result;
            }
            return await UserManager.UpdateSecurityStampAsync(User);
        }
    }
}
