using Application.Categories.Commands;

namespace NorthwindTest.Unit.CommandBuilder
{
    public class CategoryCommandBuilder
    {
        private int? Id = null;
        private string Name = "category 1 ";
        private string Description = "new desc";
        private byte[] Picture = new byte[] { 1, 2, 4, 43, 3, 2, 4 };

        public CategoryCommandBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public CategoryCommandBuilder WithId(int id)
        {
            Id = id;
            return this;
        }

        public CategoryUpsertCommand BuildAsUpsertCommand()
        => CategoryUpsertCommand.Create(Id, Name, Description, Picture);
    }
}