using MediatR;

namespace Application.Services.CategoryServices.Commands.Upsert
{
    public record UpsertCategoryCommand(int? id, string name, string description, byte[] picture, int? parentId) : IRequest
    {

    }
}