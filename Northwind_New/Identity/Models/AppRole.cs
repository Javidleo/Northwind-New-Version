using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;


namespace Identity.Models
{
    public class AppRole : IdentityRole
    {
        public AppRole()
        {
        }

        public AppRole(string name)
            : this()
        {
            Name = name;
        }

        public AppRole(string name, string description)
            : this(name)
        {
            Description = description;
        }

        public string Description { get; set; }
        public bool IsActive { get; set; } = true;


        public virtual ICollection<AppUserRole> Users { get; set; }
    }
}
