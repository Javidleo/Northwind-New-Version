using Application.Common.Exceptions;
using Application.Contracts;
using DomainModel.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.CategoryServices.Commands.Upsert
{
    public class UpsertCategoryCommandHandler : IRequestHandler<UpsertCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpsertCategoryCommandHandler(ICategoryRepository categoryRepository)
        => _categoryRepository = categoryRepository;

        public async Task<Unit> Handle(UpsertCategoryCommand request, CancellationToken cancellationToken)
        {
            if (_categoryRepository.DoesExist(i => i.CategoryName == request.name))
                throw new ConflictException("duplciate CategoryName");

            if (request.id == null) // Create Situation
            {
                var category = Category.Create(request.name, request.description, request.picture);
                _categoryRepository.Add(category);
                return Unit.Value;
            }
            else   // Update Situation
            {
                var category = await _categoryRepository.FindAsync(request.id.Value);
                if (category is null)
                    throw new NotFoundException("category notFound");

                category.Modify(request.name, request.description, request.picture);
                _categoryRepository.Update(category);
                return Unit.Value;
            }
        }
    }
}