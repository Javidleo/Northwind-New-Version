using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel
{
    public class PostAttachment
    {
        [Key]
        public long Id { get; }

        public string Title { get; private set; }

        public long PostId { get; private set; }

        public Guid UserId { get; private set; }

        public string FileSystemName { get; private set; }

        public string FileName { get; private set; }

        public string FileType { get; private set; }

        public string ContentType { get; private set; }

        public long FileSize { get; private set; }

        public string FilePath { get; private set; }

        [NotMapped]
        public byte[] File { get; private set; }

        public static PostAttachment AttachFile(string Title, long PostId, Guid UserId, string FileName, string FileType,
            string ContentType, long FileSize, string FilePath, byte[] File)
        => new(Title, PostId, UserId, FileName, FileType, ContentType, FileSystemName: MakeFileSystemName(FileName), FileSize, FilePath, File);

        private static string MakeFileSystemName(string FileName)
        => Guid.NewGuid().ToString() + FileName;

        [Obsolete]
        public PostAttachment()
        {
        }

        public void AddFilePath(string filePath)
        => FilePath = filePath;

        public PostAttachment(string Title, long PostId, Guid UserId, string FileName, string FileType, string ContentType,
            string FileSystemName, long FileSize, string FilePath, byte[] File)
        {
            this.Title = Title;
            this.ContentType = ContentType;
            this.FileName = FileName;
            this.FilePath = FilePath;
            this.FileSize = FileSize;
            this.PostId = PostId;
            this.UserId = UserId;
            this.FileSystemName = FileSystemName;
            this.FileType = FileType;
            this.File = File;
        }
    }
}