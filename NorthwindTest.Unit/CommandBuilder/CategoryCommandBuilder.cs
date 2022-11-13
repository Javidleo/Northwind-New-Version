using Application.Services.CategoryServices.Commands.Delete;
using Application.Services.CategoryServices.Commands.Upsert;
using System;

namespace NorthwindTest.Unit.CommandBuilder
{
    public class CategoryCommandBuilder
    {
        private int? _Id = null;
        public CategoryCommandBuilder WithId(int id)
        {
            _Id = id;
            return this;
        }
        public UpsertCategoryCommand BuildAsUpsertCommand()
        {
            return new UpsertCategoryCommand(_Id, "category1", "description", new byte[5] { 1, 2, 4, 5, 4 },1);
        }

        public DeleteCategoryCommand BuildAsDeleteCommand()
        {
            if (_Id == null)
                return new DeleteCategoryCommand(1);

            return new DeleteCategoryCommand(_Id.Value);
        }
    }
}