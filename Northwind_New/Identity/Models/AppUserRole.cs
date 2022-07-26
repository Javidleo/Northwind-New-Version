using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;


namespace Identity.Models
{
    public class AppUserRole : IdentityUserRole<string>
    {
        public AppUserRole()
        {

        }
        public AppUserRole(DateTime fromDate, DateTime? toDate) : this()
        {
            FromDate = fromDate;
            ToDate = toDate;
        }

        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; } = DateTime.Now.Date;

        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        [StringLength(5)]
        public string FromTime { get; set; }

        [StringLength(5)]
        public string ToTime { get; set; }

        public virtual AppUser User { get; set; }

        public virtual AppRole Role { get; set; }

    }
}
