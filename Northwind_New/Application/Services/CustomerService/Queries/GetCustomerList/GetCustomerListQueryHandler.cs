using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.CustomerService.Queries.GetCustomerList
{
    public class GetCustomerListQueryHandler : IRequestHandler<GetCustomerListQuery, List<GetCustomerListViewModel>>
    {
        private readonly IReadDbContext _context;
        private readonly IMapper _mapper;
        public GetCustomerListQueryHandler(IReadDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GetCustomerListViewModel>> Handle(GetCustomerListQuery query, CancellationToken cancellationToken)
        {
            var result = await _context.Customer
                .ProjectTo<GetCustomerListViewModel>(_mapper.ConfigurationProvider)
                .AsNoTracking().ToListAsync();

            return result;
        }
    }
}
