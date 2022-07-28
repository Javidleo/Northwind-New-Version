using QueryHandling.Abstractions;

namespace Application.Tests.Query
{
    public class TestQueryHandler : IHandleQuery<TestQuery, TestViewModel>
    {
        private readonly IReadDbContext _context;
        public TestQueryHandler(IReadDbContext context)
        {
            _context = context;
        }

        public Task<TestViewModel> Handle(TestQuery query)
        {
            var result = _context.Test.Where(i => i.Id == query.Id).Select(i => new TestViewModel
            {
                name = i.Name,
                family = i.Family,
            }).FirstOrDefault();

            return Task.FromResult(result);
        }
    }
}
