using MediatR;

namespace Application.Services.CustomerService.Queries.GetCustomerList
{
    public record GetCustomerListQuery() : IRequest<List<GetCustomerListViewModel>>;
}
