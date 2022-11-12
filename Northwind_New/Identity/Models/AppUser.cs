using Microsoft.AspNetCore.Identity;

namespace Identity.Models
{
    public class AppUser : IdentityUser<int>
    {
        public Guid Guid { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? BirthDate { get; set; }

        public bool? Gender { get; set; }

        #region Normal Create Style

        public AppUser(string firstName, string lastName, string userName, string phoneNumber, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            PhoneNumber = phoneNumber;
            Email = email;
        }

        public static AppUser Create(string firstName, string lastName, string userName, string phoneNumber, string email)
       => new(firstName, lastName, userName, phoneNumber, email);

        #endregion

        #region Google Authentication Create Style
        public AppUser(string email, string firstName, string lastName, bool emailConfirmed)
        {
            Email = email;
            FirstName = FirstName;
            LastName = lastName;
            EmailConfirmed = emailConfirmed;
        }
        public static AppUser Create(string email, string firstName, string lastName, bool emailConfirmed = true)
        => new(email, firstName, lastName, emailConfirmed);

        #endregion

        #region Sms Authentication Create Style
        private AppUser(string userName, string phoneNumber)
        {
            UserName = userName;
            PhoneNumber = phoneNumber;
        }

        public static AppUser Create(string userName, string phoneNumber)
        => new(userName, phoneNumber);

        #endregion

        public void SetInformation(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

    }
}
