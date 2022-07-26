using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity.Models
{
    public class AppUser : IdentityUser
    {
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string UserIdentity { get; set; }

        [StringLength(10)]
        public string NationalCode { get; set; }

        [StringLength(10)]
        public string BirthDate { get; set; }

        public Int16? Gender { get; set; }
        public bool IsActive { get; set; } = true;

        [NotMapped]
        public string DisplayName
        {
            get
            {
                var displayName = $"{FirstName} {LastName}";
                return string.IsNullOrWhiteSpace(displayName) ? UserName : displayName;
            }
        }

        [NotMapped]
        public string GenderTitle
        {
            get
            {
                return Gender switch
                {
                    null => " ",
                    1 => "مرد",
                    2 => "زن",
                    _ => "نامشخص",
                };
            }
        }

        public virtual ICollection<AppUserRole> Roles { get; set; }
        public virtual ICollection<UserSmsToken> UserSmsTokens { get; set; }
    }
}
