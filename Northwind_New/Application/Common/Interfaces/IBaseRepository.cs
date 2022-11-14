using System.Linq.Expressions;

namespace Application.Common.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        // Add
        void Add(TEntity entity);
        Task AddAsync(TEntity entity);
        // Update 
        void Update(TEntity entity);

        // Delete 
        void Delete(TEntity entity);

        // Find 
        Task<TEntity> FindAsync(int Id);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> filter);
        Task<List<TEntity>> FindAllAsync();

        // Search Query  take an expression and return a list of expected entity
        Task<List<TEntity>> Search(Expression<Func<TEntity, bool>> filter);


        // Does Exist // check for input query and return true if Exist and false if it is not existed 
        bool DoesExist(Expression<Func<TEntity, bool>> filter);
        Task<bool> DoesExistAsync(Expression<Func<TEntity, bool>> filter);

    }
}
