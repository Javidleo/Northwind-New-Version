using QueryHandling.Abstractions;
using ReadModels.Queries.PostAttachmentQueries.GetPostAllAttachments;
using System.Collections.Generic;

namespace ReadModels.Queries.PostQueries.GetPost
{
    public class PostViewModelWithContentOutPut : IAmAViewModel
    {
        public long Id { get; set; }

        public string PostTitle { get; set; }

        public string PostContent { get; set; }

        public long? CategoryId { get; set; }

        public string Tags { get; set; }

        public bool IsPrivate { get; set; }

        public bool IsDraft { get; set; }

        public int VisitCount { get; set; }

        public string InsertDate { get; set; }

        public long? GroupId { get; set; }

        public string GroupTitle { get; set; }

        public long? SubGroupId { get; set; }

        public string SubGroupTitle { get; set; }

        public List<UserPostAttachmentViewModelOutPut> FileAttachments { get; set; }
    }
}
