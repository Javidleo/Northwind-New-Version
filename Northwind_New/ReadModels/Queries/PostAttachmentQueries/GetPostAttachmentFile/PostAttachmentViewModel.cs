using QueryHandling.Abstractions;
using System;
using System.ComponentModel.DataAnnotations;

namespace ReadModels.Queries.PostAttachmentQueries.GetPostAttachmentFile
{
    public class PostAttachmentViewModel : IAmAViewModel
    {
        [Key]
        public long Id { get; set; }

        public string PostAttachmentTitle { get; set; }

        public long PostId { get; set; }

        public Guid UserId { get; set; }

        public string FileSystemName { get; set; }

        public string FileName { get; set; }

        public string FileType { get; set; }

        public string ContentType { get; set; }

        public long FileSize { get; set; }

        public string FilePath { get; set; }

        public byte[] ImageFileThumbnail { get; set; }

        public DateTime InsertDate { get; set; }

        public string PersianInsertDate { get; set; }

        public string InsertTime { get; set; }
    }
}
