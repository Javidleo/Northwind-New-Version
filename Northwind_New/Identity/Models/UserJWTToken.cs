using System;

namespace Identity.Models
{
    public class UserJWTToken
    {
        public int jwt_Id { get; set; }
        public string jwt_AccessTokenHash { get; set; }

        public DateTime jwt_AccessTokenExpiresDateTime { get; set; }

        public string jwt_RefreshTokenIdHash { get; set; }

        public string jwt_RefreshTokenIdHashSource { get; set; }

        public DateTime jwt_RefreshTokenExpiresDateTime { get; set; }

        public string usr_guid { get; set; }
        public string jwt_Source { get; set; }

    }
}
