using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.CustomerService.Queries.GetCustomerDetail
{
    public class GetCustomerDetailQueryHandler : IRequestHandler<GetCustomerDetailQuery, GetCustomerDetailViewModel>
    {
        private readonly IReadDbContext _context;
        private readonly IMapper _mapper;
        public GetCustomerDetailQueryHandler(IReadDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetCustomerDetailViewModel> Handle(GetCustomerDetailQuery query, CancellationToken cancellationToken)
        {
            var customer = await _context.Customer
                .Where(i => i.Id == query.Id)
                .ProjectTo<GetCustomerDetailViewModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

            return customer;
        }
    }
}
