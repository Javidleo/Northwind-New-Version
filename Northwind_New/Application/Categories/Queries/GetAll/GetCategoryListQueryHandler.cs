using Application.Common;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using QueryHandling.Abstractions;

namespace Application.Categories.Queries.GetAll
{
    public class GetCategoryListQueryHandler : IHandleQuery<GetCategoryListQuery, CategoryListViewModel>
    {
        private readonly IReadDbContext _context;
        private readonly IMapper _mapper;
        public GetCategoryListQueryHandler(IReadDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<CategoryListViewModel> Handle(GetCategoryListQuery query)
        {
            var categories = _context.Category
                .ProjectTo<CategoryViewModel>(_mapper.ConfigurationProvider).AsNoTracking().ToList();

            var result = new CategoryListViewModel
            {
                Categories = categories,
                Count = categories.Count
            };

            return Task.FromResult(result);
        }
    }
}
