using Application.Contracts;
using CommandHandling.Abstractions;
using CustomException.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Categories.Commands.Delete
{
    public class DeleteCategoryCommandHandler : IHandleCommand<DeleteCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
        => _categoryRepository = categoryRepository;

        public Task Handle(DeleteCategoryCommand command)
        {
            var category = _categoryRepository.Find(command.id);
            if (category is null)
                throw new NotFoundException("category notfound");

            if (_categoryRepository.HasProduct(command.id))
                throw new ForbiddenException("cannot delete category when you asign product to that");

            _categoryRepository.Delete(category);
            return Task.CompletedTask;
        }
    }
}
