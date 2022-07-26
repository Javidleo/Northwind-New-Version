using QueryHandling.Abstractions;
using System;
using System.ComponentModel.DataAnnotations;

namespace ReadModels.Queries.PostQueries.GetPost
{
    public class PostViewModel : IAmAViewModel
    {
        [Key]
        public long Id { get; set; }

        public string PostTitle { get; set; }

        public string PostContent { get; set; }

        public long? CategoryId { get; set; }

        public string CategoryTitle { get; set; }

        public Guid UserId { get; set; }

        public long? GroupId { get; set; }

        public string GroupTitle { get; set; }

        public long? SubGroupId { get; set; }

        public string SubGroupTitle { get; set; }

        public string Tags { get; set; }

        public bool IsPrivate { get; set; }

        public bool IsDraft { get; set; }

        public bool LogicalDelete { get; set; }

        public int VisitCount { get; set; }

        public DateTime InsertDate { get; set; }

        public string PersianInsertDate { get; set; }

        public string InsertTime { get; set; }
    }
}
