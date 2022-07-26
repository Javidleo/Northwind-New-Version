using Identity.DataSource;
using Identity.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Repositories
{
    public class UserJWTTokenRepository
    {
        readonly ApplicationDbContext Context;
        public UserJWTTokenRepository(ApplicationDbContext context)
        => Context = context;

        public void Add(UserJWTToken jWTToken)
        {
            Context.UserJWTTokens.Add(jWTToken);
            Context.SaveChanges();
        }
        public async Task AddAsync(UserJWTToken jWTToken)
        {
            await Context.UserJWTTokens.AddAsync(jWTToken);
        }

        public List<UserJWTToken> FindUserTokens(string userGuid)
        {
            return Context.UserJWTTokens.Where(x => x.usr_guid == userGuid).ToList();
        }
        public async Task<List<UserJWTToken>> FindUserTokensAsync(string userGuid)
        {
            return await Context.UserJWTTokens.Where(x => x.usr_guid == userGuid).ToListAsync();
        }
        public async Task<UserJWTToken> FindTokenbyRefreshTokenIdHashAsync(string refreshTokenIdHash)
        {
            return await Context.UserJWTTokens.FirstOrDefaultAsync(x => x.jwt_RefreshTokenIdHash == refreshTokenIdHash);
        }
        public async Task<UserJWTToken> FindTokenbyRefreshTokenIdHashAndRefreshTokenSourceAsync(string refreshTokenIdHash, string refreshTokenIdHashSource)
        {
            return await Context.UserJWTTokens.FirstOrDefaultAsync(t => t.jwt_RefreshTokenIdHash == refreshTokenIdHash
                                                                        && t.jwt_RefreshTokenIdHashSource == refreshTokenIdHashSource);
        }
        public async Task<UserJWTToken> FindUserTokenbyAccessTokenHashAsync(string userGuid, string accessTokenHash)
        {
            return await Context.UserJWTTokens.FirstOrDefaultAsync(x => x.jwt_AccessTokenHash == accessTokenHash && x.usr_guid == userGuid);
        }
        public async Task<List<UserJWTToken>> FindTokensbyRefreshTokenSourceAsync(string refreshTokenIdHashSource)
        {
            return await Context.UserJWTTokens.Where(t => t.jwt_RefreshTokenIdHashSource == refreshTokenIdHashSource)
                                               .ToListAsync();
        }
        public async Task<UserJWTToken> FindUserTokenbyRefreshTokenSourceAsync(string userGuid, string refreshTokenIdHashSource)
        {
            return await Context.UserJWTTokens.FirstOrDefaultAsync(t => t.usr_guid == userGuid && t.jwt_RefreshTokenIdHashSource == refreshTokenIdHashSource);
        }

        public void Delete(UserJWTToken userJWTToken)
        {
            Context.UserJWTTokens.Remove(userJWTToken);
        }
        public async Task RemoveTokensbyRefreshTokenSourceAsync(string refreshTokenIdHashSource)
        {
            await Context.UserJWTTokens.Where(t => t.jwt_RefreshTokenIdHashSource == refreshTokenIdHashSource)
               .ForEachAsync(userToken => Context.UserJWTTokens.Remove(userToken));
        }
        public async Task RemoveUserTokenbyRefreshTokenSourceAsync(string userGuid, string refreshTokenIdHashSource)
        {
            var userToken = await FindUserTokenbyRefreshTokenSourceAsync(userGuid, refreshTokenIdHashSource);
            //Context.UserJWTTokens.FirstOrDefaultAsync(t => t.usr_guid == userGuid && t.jwt_RefreshTokenIdHashSource == refreshTokenIdHashSource);
            if (userToken != null)
                Context.UserJWTTokens.Remove(userToken);
        }
        public async Task RemoveUserTokensAsync(string userGuid)
        {
            await Context.UserJWTTokens.Where(x => x.usr_guid == userGuid)
                          .ForEachAsync(userToken => Context.UserJWTTokens.Remove(userToken));
        }

        public void Save()
        {
            Context.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }

    }
}
