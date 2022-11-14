using MediatR;

namespace NorthwindTest.Unit.Test.Test
{
    public record CreateProductCommand(string productName, decimal price, int? suplierId, int? categoryId, bool discounted) : IRequest
    {
    }
}