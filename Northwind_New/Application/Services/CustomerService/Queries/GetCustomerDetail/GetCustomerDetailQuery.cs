using MediatR;

namespace Application.Services.CustomerService.Queries.GetCustomerDetail
{
    public record GetCustomerDetailQuery(int Id) : IRequest<GetCustomerDetailViewModel>;
}
