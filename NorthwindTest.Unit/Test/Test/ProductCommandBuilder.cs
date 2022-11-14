namespace NorthwindTest.Unit.Test.Test
{
    internal class ProductCommandBuilder
    {
        public ProductCommandBuilder()
        {
        }

        public CreateProductCommand BuildAsCreateCommand()
        {
            return new CreateProductCommand("name", 1000, 1, 1, false);
        }
    }
}