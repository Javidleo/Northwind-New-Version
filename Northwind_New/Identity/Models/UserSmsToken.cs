using System;

namespace Identity.Models
{
    public class UserSmsToken
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Description { get; set; }

        public virtual AppUser User { get; set; }
    }
}
