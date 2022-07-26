using QueryHandling.Abstractions;

namespace ReadModels.Queries.PostAttachmentQueries.GetPostAllAttachments
{
    public class UserPostAttachmentViewModelOutPut : IAmAViewModel
    {
        public long Id { get; set; }

        public string PostAttachmentTitle { get; set; }

        public byte[] ImageFileThumbnail { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }
    }
}
