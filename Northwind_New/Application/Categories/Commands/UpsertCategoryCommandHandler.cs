using Application.Contracts;
using CommandHandling.Abstractions;
using CustomException.Exceptions;
using DomainModel.Entities;

namespace Application.Categories.Commands
{
    public class UpsertCategoryCommandHandler : IHandleCommand<CategoryUpsertCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        public UpsertCategoryCommandHandler(ICategoryRepository categoryRepository)
        => _categoryRepository = categoryRepository;
       

        public async Task Handle(CategoryUpsertCommand command)
        {
            if (command.id.HasValue)
            {
                var category = await _categoryRepository.Find(command.id.Value);
                if (category is null)
                    throw new NotFoundException("category not founded");

                category.ChangeProperties(command.Name, command.Description, command.Picture);
                _categoryRepository.Update(category);
            }

            else
            {
                Category category = Category.Create(command.Name, command.Description, command.Picture);
                _categoryRepository.Add(category);
            }

        }
    }
}