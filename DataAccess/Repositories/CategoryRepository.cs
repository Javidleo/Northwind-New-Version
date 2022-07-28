using Application.Contracts;
using DomainModel.Entities;

namespace DataAccess.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IWriteDbContext _context;
        public CategoryRepository(IWriteDbContext context)
        => _context = context;

        public void Add(Category category)
        => _context.Category.Add(category);

        public async Task<Category> Find(int id)
        => await _context.Category.FindAsync(id);

        public void Update(Category category)
        => _context.Category.Update(category);
    }
}
