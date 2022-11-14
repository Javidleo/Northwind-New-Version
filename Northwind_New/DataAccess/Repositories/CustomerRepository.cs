using Application.Contracts;
using DomainModel.Entities;

namespace DataAccess.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(IWriteDbContext context) : base(context)
        {
        }

        public bool DoesPhoneNumberExist(string phone)
        => _context.Customer.Any(i => i.PhoneNumber == phone);
    }
}
