using Identity.DataSource;
using Identity.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Repositories
{
    public class UserSmsTokenRepository
    {
        readonly ApplicationDbContext Context;
        public UserSmsTokenRepository(ApplicationDbContext context)
        => Context = context;

        public void Add(UserSmsToken smsToken)
        {
            Context.UserSmsTokens.Add(smsToken);
            Context.SaveChanges();
        }

        public async Task AddAsync(UserSmsToken smsToken)
        {
            await Context.UserSmsTokens.AddAsync(smsToken);
            await Context.SaveChangesAsync();
        }

        public UserSmsToken GetLastbyUserId(string userId)
        {
            return Context.UserSmsTokens.OrderBy(c => c.CreateDate).LastOrDefault(c => c.UserId == userId);
        }

        public UserSmsToken GetLastbyPhoneNumber(string phoneNumber)
        {
            return Context.UserSmsTokens.OrderBy(c => c.CreateDate).LastOrDefault(c => c.PhoneNumber == phoneNumber);
        }
    }
}
