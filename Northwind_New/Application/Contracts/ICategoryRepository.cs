using Application.Common.Interfaces;
using DomainModel.Entities;

namespace Application.Contracts
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        bool HasChildren(int parentId);
        bool HasProduct(int categoryId);
    }
}