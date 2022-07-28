using DomainModel.Entities;

namespace Application.Contracts
{
    public interface ITestRepository
    {
        void Add(Test test);
    }
}