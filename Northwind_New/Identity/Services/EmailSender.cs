using Identity.Models;
using Identity.Services.Contracts;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Identity.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string message)
        {
            //SmtpClient client = new SmtpClient();
            //client.DeliveryMethod = SmtpDeliveryMethod.
            // SpecifiedPickupDirectory;
            //client.PickupDirectoryLocation = @"C:\Test";

            //client.Send("test@localhost", user.Email,
            //       "Confirm your email",
            //   confirmationLink);

            EmailModel model = new EmailModel
            {
                Email = "negso.info@gmail.com",
                Password = "rdhkqksrtvoxpogu",
                Subject = "تایید ایمیل در سامانه مدیریت دانش",
                Body = message,
                To = email
            };

            using (MailMessage mm = new MailMessage(model.Email, model.To))
            {
                mm.Subject = model.Subject;
                mm.Body = model.Body;
                mm.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    NetworkCredential NetworkCred = new NetworkCredential(model.Email, model.Password);
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                    // ViewBag.Message = "Email sent.";
                }
            }

            return Task.CompletedTask;
        }
    }
}
