using Application.Contracts;
using DomainModel.Entities;

namespace DataAccess.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IWriteDbContext _context;
        private readonly CancellationToken _token;
        public CategoryRepository(IWriteDbContext context, CancellationToken token)
        {
            _context = context;
            _token = token;
        }

        public void Add(Category category)
        {
            _context.Category.Add(category);
            _context.SaveChangesAsync(_token);
        }

        public async Task<Category> Find(int id)
        => await _context.Category.FindAsync(id);

        public void Update(Category category)
        {
            _context.Category.Update(category);
            _context.SaveChangesAsync(_token);
        }
    }
}
