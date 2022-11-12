using Application.Common.Interfaces;
using DomainModel.Entities;

namespace Application.Contracts
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        bool HasProduct(int categoryId);
    }
}