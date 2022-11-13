using System.Threading;
using System.Threading.Tasks;

namespace DataSource
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
