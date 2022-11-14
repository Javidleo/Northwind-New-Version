using Xunit;

namespace NorthwindTest.Unit.Test.Test
{
    public class ProductTests
    {
        [Fact]
        public void CreateProduct_CheckForInvalidCategoryId_ThrowsNotFoundException()
        {
            var command = new ProductCommandBuilder().BuildAsCreateCommand();
        }
    }
}
