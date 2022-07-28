using DomainModel.Entities;

namespace Application.Contracts
{
    public interface ICategoryRepository
    {
        void Add(Category category);
        Task<Category> Find(int id);
        void Update(Category category);
    }
}