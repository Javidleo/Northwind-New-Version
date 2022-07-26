using QueryHandling.Abstractions;

namespace ReadModels.Queries.PostQueries.GetPostList
{
    public class PostViewModelOutPut : IAmAViewModel
    {
        public long Id { get; set; }

        public string PostTitle { get; set; }

        public string CategoryTitle { get; set; }

        public string Tags { get; set; }

        public bool IsPrivate { get; set; }

        public bool IsDraft { get; set; }

        public int VisitCount { get; set; }

        public string InsertDate { get; set; }

        public long? GroupId { get; set; }

        public string GroupTitle { get; set; }

        public long? SubGroupId { get; set; }

        public string SubGroupTitle { get; set; }
    }
}
