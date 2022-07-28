using Application;
using LinqKit;
using NEGSO.Utilities;
using QueryHandling.Abstractions;
using ReadModels.Common;
using ReadModels.Queries.PostQueries.GetPost;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReadModels.Queries.PostQueries.GetPostList
{
    public record GetPostListQuery(int PageNumber, int PageSize, long? CategoryId, long? GroupId, long? SubGroupId, string PostTitle,
                                        string PostContent, string Tags, bool? Private, bool? Draft, string StartDate, string EndDate,
                                        string SortOrder) : Query<PagedViewModel<PostViewModelOutPut>>
    {


        public class GetPostListQueryHandler : IHandleQuery<GetPostListQuery, PagedViewModel<PostViewModelOutPut>>
        {
            private readonly IReadDbContext readDbContext;
            public GetPostListQueryHandler(IReadDbContext readDbContext)
            => this.readDbContext = readDbContext;

            Task<PagedViewModel<PostViewModelOutPut>> IHandleQuery<GetPostListQuery, PagedViewModel<PostViewModelOutPut>>.Handle(GetPostListQuery query)
            {
                DateTime? FromDate = query.StartDate.ToGregorianDate();
                DateTime? ToDate = query.EndDate.ToGregorianDate();
                string[] Tags = (query.Tags ?? string.Empty).Split(new char[',']);
                IQueryable<PostViewModel> TotalItems = readDbContext.PostViewModels.AsQueryable();
                if (!string.IsNullOrEmpty(query.PostTitle))
                    TotalItems = TotalItems.Where(c => c.PostTitle.Contains(query.PostTitle));
                if (!string.IsNullOrEmpty(query.PostContent))
                    TotalItems = TotalItems.Where(c => c.PostTitle.Contains(query.PostContent));
                if (query.CategoryId != null)
                    TotalItems = TotalItems.Where(c => c.CategoryId == query.CategoryId);
                if (query.GroupId != null)
                    TotalItems = TotalItems.Where(c => c.GroupId == query.GroupId);
                if (query.SubGroupId != null)
                    TotalItems = TotalItems.Where(c => c.SubGroupId == query.SubGroupId);
                if (FromDate != null)
                    TotalItems = TotalItems.Where(c => c.InsertDate.Date >= FromDate);
                if (ToDate != null)
                    TotalItems = TotalItems.Where(c => c.InsertDate.Date <= ToDate);
                if (query.Private != null)
                    TotalItems = TotalItems.Where(c => c.IsPrivate == query.Private);
                if (query.Draft != null)
                    TotalItems = TotalItems.Where(c => c.IsDraft == query.Draft);

                var predicate = PredicateBuilder.New<PostViewModel>();
                foreach (string tag in Tags)
                    predicate = predicate.Or(x => x.Tags.Contains(tag.Trim()));
                //var sql = TotalItems.ToQueryString();
                TotalItems = TotalItems.Where(predicate);
                TotalItems = query.SortOrder switch
                {
                    "posttitle" => TotalItems.OrderBy(c => c.PostTitle),
                    "posttitle_desc" => TotalItems.OrderByDescending(c => c.PostTitle),
                    "visitcount" => TotalItems.OrderBy(c => c.VisitCount),
                    "visitcount_desc" => TotalItems.OrderByDescending(c => c.VisitCount),
                    "insertdate" => TotalItems.OrderBy(t => t.InsertDate),
                    "insertdate_desc" => TotalItems.OrderByDescending(t => t.InsertDate),
                    "isprivate" => TotalItems.OrderBy(t => t.IsPrivate),
                    "isprivate_desc" => TotalItems.OrderByDescending(t => t.IsPrivate),
                    "isdraft" => TotalItems.OrderBy(t => t.IsDraft),
                    "isdraft_desc" => TotalItems.OrderByDescending(t => t.IsDraft),
                    "groupid" => TotalItems.OrderBy(t => t.GroupId),
                    "groupid_desc" => TotalItems.OrderByDescending(t => t.GroupId),
                    "subgroupid" => TotalItems.OrderBy(t => t.SubGroupId),
                    "subgroupid_desc" => TotalItems.OrderByDescending(t => t.SubGroupId),
                    _ => TotalItems.OrderByDescending(c => c.InsertDate)
                };

                var TotalItemsOutPut = TotalItems.Select(c => new PostViewModelOutPut
                {
                    CategoryTitle = c.CategoryTitle,
                    InsertDate = c.PersianInsertDate + " " + c.InsertTime,
                    Id = c.Id,
                    PostTitle = c.PostTitle,
                    VisitCount = c.VisitCount,
                    Tags = c.Tags,
                    GroupId = c.GroupId,
                    GroupTitle = c.GroupTitle,
                    SubGroupId = c.SubGroupId,
                    SubGroupTitle = c.SubGroupTitle,
                    IsPrivate = c.IsPrivate,
                    IsDraft = c.IsDraft
                });

                var totalRecords = TotalItemsOutPut.Count();
                var result = PagingUtility.Paginate(query.PageNumber, query.PageSize, TotalItemsOutPut);
                return Task.FromResult(result);
            }
        }

    }
}
