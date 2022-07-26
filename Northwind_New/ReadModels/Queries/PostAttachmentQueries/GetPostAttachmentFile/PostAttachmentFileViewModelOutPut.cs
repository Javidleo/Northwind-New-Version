using QueryHandling.Abstractions;

namespace ReadModels.Queries.PostAttachmentQueries.GetPostAttachmentFile
{
    public class PostAttachmentFileViewModelOutPut : IAmAViewModel
    {
        public long Id { get; set; }

        public byte[] File { get; set; }
    }
}
