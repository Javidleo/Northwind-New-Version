using Application.Contracts;
using DomainModel.Entities;

namespace DataAccess.Repositories
{
    public class TestRepository : ITestRepository
    {
        private readonly IWriteDbContext _dbContext;
        public TestRepository(IWriteDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(Test test)
        {
            _dbContext.Test.Add(test);
        }
    }
}
