using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly IWriteDbContext _context;
        private readonly DbSet<TEntity> _dbSet;
        public BaseRepository(IWriteDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public virtual void Add(TEntity entity)
        {
            _dbSet.Add(entity);
            _context.Entry(entity).State = EntityState.Added;
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            _context.Entry(entity).State = EntityState.Added;
        }

        public virtual void Delete(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);

            _dbSet.Remove(entity);
        }

        public virtual async Task<bool> DoesExist(Expression<Func<TEntity, bool>> filter)
        => await _dbSet.AnyAsync(filter);

        public virtual async Task<List<TEntity>> FindAllAsync()
        => await _dbSet.AsNoTracking().ToListAsync();

        public virtual async Task<TEntity> FindAsync(int Id)
        => await _dbSet.FindAsync(Id) ?? throw new ArgumentNullException();

        public virtual async Task<List<TEntity>> Search(Expression<Func<TEntity, bool>> filter)
        => await _dbSet.Where(filter).AsNoTracking().ToListAsync();

        public virtual void Update(TEntity entity)
        {
            _dbSet.Update(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
