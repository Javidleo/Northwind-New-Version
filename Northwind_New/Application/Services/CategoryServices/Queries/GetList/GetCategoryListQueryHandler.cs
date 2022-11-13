using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.CategoryServices.Queries.GetList
{
    public class GetCategoryListQueryHandler : IRequestHandler<GetCategoryListQuery, List<GetCategoryListViewModel>>
    {
        private readonly IReadDbContext _context;
        private readonly IMapper _mapper;

        public GetCategoryListQueryHandler(IReadDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<GetCategoryListViewModel>> Handle(GetCategoryListQuery query, CancellationToken cancellationToken)
        {
            var result = await _context.Category
                .ProjectTo<GetCategoryListViewModel>(_mapper.ConfigurationProvider).ToListAsync();

            return result;
        }
    }
}
