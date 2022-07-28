using Application.Contracts;
using DomainModel.Entities;

namespace DataAccess.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IWriteDbContext _context;
        private readonly CancellationToken _token = new CancellationToken();
        public CategoryRepository(IWriteDbContext context)
        {
            _context = context;
        }

        public void Add(Category category)
        {
            _context.Category.Add(category);
            _context.SaveChangesAsync(_token);
        }

        public Category Find(int id)
        => _context.Category.Find(id);

        public bool HasProduct(int categoryId)
        => _context.Product.Any(i => i.CategoryId == categoryId);

        public void Update(Category category)
        {
            _context.Category.Update(category);
            _context.SaveChangesAsync(_token);
        }

        public void Delete(Category category)
        {
            _context.Category.Remove(category);
            _context.SaveChangesAsync(_token);
        }
    }
}
