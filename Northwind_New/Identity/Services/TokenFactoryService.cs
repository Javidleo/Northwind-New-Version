using Identity.Models;
using Identity.Services.Contracts;
using Identity.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Services
{
    public class JwtTokensData
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string RefreshTokenSerial { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
    }
    public class TokenFactoryService   //: ITokenFactoryService
    {
        readonly SecurityService SecurityService;
        //readonly UserRoleRepository UserRoles;
        //readonly UserRepository Users;
        readonly IOptionsSnapshot<BearerTokens> _Configuration;
        readonly IAppUserManager UserManager;

        public TokenFactoryService(
            SecurityService securityService,
            //UserRoleRepository userRoles,
            // UserRepository users,
            IOptionsSnapshot<BearerTokens> configuration,
            IAppUserManager userManager)
        {
            SecurityService = securityService;
            //  SecurityService.CheckArgumentIsNull(nameof(SecurityService));
            //UserRoles = userRoles;
            //UserRoles.CheckArgumentIsNull(nameof(UserRoles));
            //Users = users;
            //Users.CheckArgumentIsNull(nameof(Users));
            _Configuration = configuration;
            // _Configuration.CheckArgumentIsNull(nameof(_Configuration));
            UserManager = userManager;
        }

        public async Task<JwtTokensData> CreateJwtTokensAsync(AppUser user)
        {
            var (accessToken, claims) = await CreateAccessTokenAsync(user);
            var (refreshTokenValue, refreshTokenSerial) = CreateRefreshToken();
            return new JwtTokensData
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue,
                RefreshTokenSerial = refreshTokenSerial,
                Claims = claims
            };
        }

        private async Task<(string AccessToken, IEnumerable<Claim> Claims)> CreateAccessTokenAsync(AppUser user)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration.Value.Key));
            var authEncryptionkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration.Value.EncryptKey));


            // var serialNumber =await Users.GetUserTrackingSerialNumberAsync(user.usr_guid);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id, ClaimValueTypes.String, _Configuration.Value.Issuer),
                    new Claim(ClaimTypes.Name, user.UserName, ClaimValueTypes.String, _Configuration.Value.Issuer),
                    new Claim(ClaimTypes.GivenName, user.DisplayName, ClaimValueTypes.String, _Configuration.Value.Issuer),
                     // Unique Id for all Jwt tokes
                    new Claim(JwtRegisteredClaimNames.Jti, SecurityService.CreateCryptographicallySecureGuid().ToString(), ClaimValueTypes.String, _Configuration.Value.Issuer),
                     // Issuer
                    new Claim(JwtRegisteredClaimNames.Iss, _Configuration.Value.Issuer, ClaimValueTypes.String, _Configuration.Value.Issuer),
                    // Issued at
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, _Configuration.Value.Issuer),
                     // to invalidate the user
                    new Claim(ClaimTypes.SerialNumber, user.SecurityStamp, ClaimValueTypes.String, _Configuration.Value.Issuer),
            };



            var userRolesList = await UserManager.GetRolesAsync(user);

            foreach (var userRole in userRolesList)
            {
                //string roleName = UserRoles.GetRoleName(userRole.rol_id);
                authClaims.Add(new Claim(ClaimTypes.Role, userRole, ClaimValueTypes.String, _Configuration.Value.Issuer));
            }

            var now = DateTime.UtcNow;
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _Configuration.Value.Issuer,
                Audience = _Configuration.Value.Audience,
                IssuedAt = now,
                NotBefore = now,
                Expires = now.AddMinutes(_Configuration.Value.AccessTokenExpirationMinutes),
                Subject = new ClaimsIdentity(authClaims),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                EncryptingCredentials = new EncryptingCredentials(authEncryptionkey, SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(descriptor);
            string encryptedJwt = tokenHandler.WriteToken(securityToken);

            //TokenViewModel Response = new TokenViewModel()
            //{
            //    Token = encryptedJwt,
            //    Expiration = descriptor.Expires.Value
            //};

            //return Response;
            return (encryptedJwt, authClaims);
        }

        private (string RefreshTokenValue, string RefreshTokenSerial) CreateRefreshToken()
        {
            var refreshTokenSerial = SecurityService.CreateCryptographicallySecureGuid().ToString().Replace("-", "");

            var claims = new List<Claim>
            {
                // Unique Id for all Jwt tokes
                new Claim(JwtRegisteredClaimNames.Jti, SecurityService.CreateCryptographicallySecureGuid().ToString(), ClaimValueTypes.String, _Configuration.Value.Issuer),
                // Issuer
                new Claim(JwtRegisteredClaimNames.Iss, _Configuration.Value.Issuer, ClaimValueTypes.String, _Configuration.Value.Issuer),
                // Issued at
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, _Configuration.Value.Issuer),
                // for invalidation
                new Claim(ClaimTypes.SerialNumber, refreshTokenSerial, ClaimValueTypes.String, _Configuration.Value.Issuer)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration.Value.Key));
            var now = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                issuer: _Configuration.Value.Issuer,
                audience: _Configuration.Value.Audience,
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(_Configuration.Value.RefreshTokenExpirationMinutes),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            var refreshTokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return (refreshTokenValue, refreshTokenSerial);
        }

        public string GetRefreshTokenSerial(string refreshTokenValue)
        {
            if (string.IsNullOrWhiteSpace(refreshTokenValue))
            {
                return null;
            }

            ClaimsPrincipal decodedRefreshTokenPrincipal = null;
            try
            {
                decodedRefreshTokenPrincipal = new JwtSecurityTokenHandler().ValidateToken(
                    refreshTokenValue,
                    new TokenValidationParameters
                    {
                        RequireExpirationTime = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration.Value.Key)),
                        ValidateIssuerSigningKey = true, // verify signature to avoid tampering
                        ValidateLifetime = true, // validate the expiration
                        ClockSkew = TimeSpan.Zero // tolerance for the expiration date
                    },
                    out _
                );
            }
            catch (Exception)
            {
                // _logger.LogError(ex, $"Failed to validate refreshTokenValue: `{refreshTokenValue}`.");
                throw new SecurityTokenValidationException($"Failed to validate refreshTokenValue: `{refreshTokenValue}`.");
            }

            return decodedRefreshTokenPrincipal?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber)?.Value;
        }

    }
}
