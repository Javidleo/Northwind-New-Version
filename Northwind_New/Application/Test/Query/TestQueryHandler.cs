using QueryHandling.Abstractions;

namespace Application.Test.Query
{
    public class TestQueryHandler : IHandleQuery<TestQuery, TestViewModel>
    {
        public Task<TestViewModel> Handle(TestQuery query)
        {
            var result = new TestViewModel();
            return Task.FromResult(result);
        }
    }
}
