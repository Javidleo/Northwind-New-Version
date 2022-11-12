using Application.Contracts;
using DomainModel.Entities;

namespace DataAccess.Repositories
{
    public class CategoryRepository : BaseRepository<Category>,ICategoryRepository
    {
        public CategoryRepository(IWriteDbContext context) : base(context)
        {
        }

        public bool HasProduct(int categoryId)
        => _context.Product.Any(i => i.CategoryId == categoryId);

    }
}
