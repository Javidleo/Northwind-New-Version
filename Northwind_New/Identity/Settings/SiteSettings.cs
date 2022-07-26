using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Settings
{
    public class SiteSettings
    {
        public AdminUserSeed AdminUserSeed { get; set; }
        public BearerTokens BearerTokens { get; set; }
        public LoginOptions LoginOptions { get; set; }
        public double SmsConfirmationLifespan { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }

    }
}
