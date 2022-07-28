using QueryHandling.Abstractions;

namespace Application.Tests.Query
{
    public record TestQuery(int Id) : Query<TestViewModel>;
}
