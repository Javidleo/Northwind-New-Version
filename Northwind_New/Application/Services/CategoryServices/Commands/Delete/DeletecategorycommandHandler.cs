using Application.Common.Exceptions;
using Application.Contracts;
using MediatR;

namespace Application.Services.CategoryServices.Commands.Delete
{
    public class DeletecategorycommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;

        public DeletecategorycommandHandler(ICategoryRepository categoryRepository)
        => _categoryRepository = categoryRepository;

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.FindAsync(request.Id);
            if (category is null)
                throw new NotFoundException("category notfound");

            if (_categoryRepository.HasChildren(request.Id))
                throw new NotAcceptableException("this category have some categories as children, " +
                    "you cannot delete this category until they are existing");

            _categoryRepository.Delete(category);
            return Unit.Value;
        }
    }
}