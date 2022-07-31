using DomainModel.Entities;

namespace Application.Contracts
{
    public interface ICustomerRepository
    {
        bool DoesPhoneNumberExist(string phone);
        void Add(Customer customer);
    }
}