using Microsoft.EntityFrameworkCore;
using QueryHandling.Abstractions;
using ReadModels.Common;
using System.Linq;
using System.Threading.Tasks;

namespace ReadModels.Queries.PostAttachmentQueries.GetPostAllAttachments
{
    public record GetPostAllAttachmentsQuery(int PageNumber, int PageSize, long PostId) :
                                                                     Query<PagedViewModel<UserPostAttachmentViewModelOutPut>>
    {


        public class GetPostAllAttachmentsQueryHandler : IHandleQuery<GetPostAllAttachmentsQuery,
                                                            PagedViewModel<UserPostAttachmentViewModelOutPut>>
        {
            private readonly IReadDbContext _context;
            public GetPostAllAttachmentsQueryHandler(IReadDbContext context)
            => _context = context;

            public Task<PagedViewModel<UserPostAttachmentViewModelOutPut>> Handle(GetPostAllAttachmentsQuery query)
            {
                var UserPostAttachmentViewModel =
                    _context.PostAttachmentViewModels.Where(c => c.PostId == query.PostId)
                        .Select(c => new UserPostAttachmentViewModelOutPut()
                        {
                            FileName = c.FileName,
                            Id = c.Id,
                            PostAttachmentTitle = c.PostAttachmentTitle
                        }).AsNoTracking();

                var result = PagingUtility.Paginate(query.PageNumber, query.PageSize, UserPostAttachmentViewModel);
                return Task.FromResult(result);
            }
        }

    }
}
