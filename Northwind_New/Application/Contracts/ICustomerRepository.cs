using Application.Common.Interfaces;
using DomainModel.Entities;

namespace Application.Contracts
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        bool DoesPhoneNumberExist(string phone);
    }
}