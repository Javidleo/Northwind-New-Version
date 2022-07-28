using DomainModel.Entities;

namespace Application.Contracts
{
    public interface ICategoryRepository
    {
        void Add(Category category);
        Category Find(int id);
        void Update(Category category);
        bool HasProduct(int categoryId);
        void Delete(Category category);
    }
}