using Identity.Models;
using Identity.Repositories;
using Identity.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;


namespace Identity.Services
{
    public class TokenStoreService
    {
        readonly IOptionsSnapshot<BearerTokens> _Configuration;
        readonly UserJWTTokenRepository UserTokens;
        readonly SecurityService SecurityService;
        readonly TokenFactoryService TokenFactoryService;
        public TokenStoreService(
            IOptionsSnapshot<BearerTokens> configuration,
            UserJWTTokenRepository userTokens,
            SecurityService securityService,
            TokenFactoryService tokenFactoryService)
        {
            _Configuration = configuration;
            UserTokens = userTokens;
            SecurityService = securityService;
            TokenFactoryService = tokenFactoryService;
        }

        public async Task AddUserTokenAsync(UserJWTToken userToken)
        {
            try
            {
                ////توکن های ایجاد شده برای کاربر حذف می شود چون هر کاربر فقط می تواند یک لاگین در یک زمان داشته باشد  
                // if (!_Configuration.Value.AllowMultipleLoginsFromTheSameUser)
                await InvalidateUserTokensAsync(userToken.usr_guid);

                ////TO DO
                ///کاربر بتواند یک لاگین از وب داشته باشد و یک لاگین از روی اپ موبایل
                ///


                ////ابتدا همه توکن های ایجاد شده برای سورس جاری حذف می شود چون از هر سورس یا دیوایس همزمان یک نفر می تواند لاگین کند
                await DeleteTokensWithSameRefreshTokenSourceAsync(userToken.jwt_RefreshTokenIdHashSource);
                //else
                //    await DeleteUserTokenWithRefreshTokenSourceAsync(userToken.usr_guid,userToken.jwt_RefreshTokenIdHashSource);
                await UserTokens.AddAsync(userToken);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task AddUserTokenAsync(AppUser user, string refreshTokenSerial, string accessToken, string refreshTokenSourceSerial)
        {
            var now = DateTime.UtcNow;
            var token = new UserJWTToken
            {
                usr_guid = user.Id,
                // Refresh token handles should be treated as secrets and should be stored hashed
                jwt_RefreshTokenIdHash = SecurityService.GetSha256Hash(refreshTokenSerial),
                jwt_RefreshTokenIdHashSource = SecurityService.GetSha256Hash(refreshTokenSourceSerial),
                jwt_AccessTokenHash = SecurityService.GetSha256Hash(accessToken),
                jwt_RefreshTokenExpiresDateTime = now.AddMinutes(_Configuration.Value.RefreshTokenExpirationMinutes),
                jwt_AccessTokenExpiresDateTime = now.AddMinutes(_Configuration.Value.AccessTokenExpirationMinutes),
                jwt_Source = refreshTokenSourceSerial
            };
            await AddUserTokenAsync(token);
        }
        public async Task InvalidateUserTokensAsync(string userGuid)
        => await UserTokens.RemoveUserTokensAsync(userGuid);
        public async Task DeleteTokensWithSameRefreshTokenSourceAsync(string refreshTokenIdHashSource)
        {
            if (string.IsNullOrWhiteSpace(refreshTokenIdHashSource))
            {
                return;
            }

            await UserTokens.RemoveTokensbyRefreshTokenSourceAsync(refreshTokenIdHashSource);
            //List<UserJWTToken> tokenList =await UserTokens.FindTokensbyRefreshTokenSourceAsync(refreshTokenIdHashSource);

            //  tokenList.ForEach(userToken => UserTokens.Delete(userToken));
        }
        public async Task DeleteUserTokenWithRefreshTokenSourceAsync(string userGuid, string refreshTokenIdHashSource)
        {
            var token = await UserTokens.FindUserTokenbyRefreshTokenSourceAsync(userGuid, refreshTokenIdHashSource);
            if (token != null)
                UserTokens.Delete(token);
            //  await UserTokens.RemoveUserTokenbyRefreshTokenSourceAsync(userGuid, refreshTokenIdHashSource);
        }
        public Task<UserJWTToken> FindTokenbyRefreshTokenAsync(string refreshTokenValue)
        {
            if (string.IsNullOrWhiteSpace(refreshTokenValue))
            {
                return Task.FromResult<UserJWTToken>(null);
            }

            var refreshTokenSerial = TokenFactoryService.GetRefreshTokenSerial(refreshTokenValue);
            if (string.IsNullOrWhiteSpace(refreshTokenSerial))
            {
                return Task.FromResult<UserJWTToken>(null);
            }

            var refreshTokenIdHash = SecurityService.GetSha256Hash(refreshTokenSerial);
            return UserTokens.FindTokenbyRefreshTokenIdHashAsync(refreshTokenIdHash);
        }

        //public Task<UserJWTToken> FindTokenbyRefreshTokenAndTokenSourceAsync(string refreshTokenValue)
        //{
        //    if (string.IsNullOrWhiteSpace(refreshTokenValue))
        //    {
        //        return Task.FromResult<UserJWTToken>(null);
        //    }

        //    var refreshTokenSerial = TokenFactoryService.GetRefreshTokenSerial(refreshTokenValue);
        //    if (string.IsNullOrWhiteSpace(refreshTokenSerial))
        //    {
        //        return Task.FromResult<UserJWTToken>(null);
        //    }

        //    var refreshTokenIdHash = SecurityService.GetSha256Hash(refreshTokenSerial);
        //    return UserTokens.FindUserTokensbyRefreshTokenIdHashWithSameSourceAsync(refreshTokenIdHash);
        //}

        public async Task<bool> IsValidTokenAsync(string accessToken, string userId)
        {
            var accessTokenHash = SecurityService.GetSha256Hash(accessToken);
            var userToken = await UserTokens.FindUserTokenbyAccessTokenHashAsync(userId, accessTokenHash);
            return userToken?.jwt_AccessTokenExpiresDateTime >= DateTime.UtcNow;
        }
        public async Task RevokeUserBearerTokensAsync(string userGuid, string refreshTokenValue)
        {
            if (!string.IsNullOrWhiteSpace(userGuid))
            {
                if (_Configuration.Value.AllowSignoutAllUserActiveClients)
                {
                    await InvalidateUserTokensAsync(userGuid);
                    return;
                }

            }
            if (!string.IsNullOrWhiteSpace(refreshTokenValue))
            {
                var jwtToken = await FindTokenbyRefreshTokenAsync(refreshTokenValue);
                if (_Configuration.Value.AllowSignoutAllUserActiveClients)
                    await InvalidateUserTokensAsync(jwtToken.usr_guid);
                else
                    // await DeleteTokensWithSameRefreshTokenSourceAsync(jwtToken.jwt_RefreshTokenIdHashSource);
                    UserTokens.Delete(jwtToken);
            }



            /////همه توکن های منقضی مربوط به همه یوزرها را حذف می کند
            // await DeleteExpiredTokensAsync();
        }

        /////همه توکن های منقضی مربوط به همه یوزرها را حذف می کند
        //public async Task DeleteExpiredTokensAsync()
        //{
        //    var now = DateTimeOffset.UtcNow;
        //    await _tokens.Where(x => x.RefreshTokenExpiresDateTime < now)
        //                .ForEachAsync(userToken => _tokens.Remove(userToken));
        //}

        public bool IsValidTokenSource(UserJWTToken userJWTToken, string currentRefreshTokenSourceSerial)
        {
            if (_Configuration.Value.AllowMultipleLoginsFromTheSameUser)
            {
                string CurrentSourceHash = SecurityService.GetSha256Hash(currentRefreshTokenSourceSerial);
                if (userJWTToken.jwt_RefreshTokenIdHashSource != CurrentSourceHash)
                    return false;
            }
            return true;
        }

    }
}
