using Application.Categories.Commands.Delete;
using Application.Categories.Commands.Upsert;
using System;

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

        public UpsertCategoryCommand BuildAsUpsertCommand()
        => UpsertCategoryCommand.Create(Id, Name, Description, Picture);

        public DeleteCategoryCommand BuildAsDeleteCommand()
        => DeleteCategoryCommand.Create(Id.Value);
    }
}