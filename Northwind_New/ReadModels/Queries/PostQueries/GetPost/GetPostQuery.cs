using QueryHandling.Abstractions;
using System;

namespace ReadModels.Queries.PostQueries.GetPost
{
    public record GetPostQuery(long Id, Guid UserId) : Query<PostViewModelWithContentOutPut>;
}
