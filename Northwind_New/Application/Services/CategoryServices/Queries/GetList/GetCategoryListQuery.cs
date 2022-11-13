using MediatR;

namespace Application.Services.CategoryServices.Queries.GetList
{
    public record GetCategoryListQuery() : IRequest<List<GetCategoryListViewModel>>;
}
