using MediatR;

namespace Application.Services.CategoryServices.Commands.Delete
{
    public record DeleteCategoryCommand(int Id) : IRequest
    {
    }
}