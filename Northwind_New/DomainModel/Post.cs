using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DomainModel
{
    public class Post //: AggregateRoot
    {
        [Key]
        public long Id { get; }

        public string PostTitle { get; private set; }

        public string PostContent { get; set; }

        public long? CategoryId { get; private set; }

        public Guid UserId { get; private set; }

        public bool IsPrivate { get; private set; }

        public bool IsDraft { get; private set; }

        public bool LogicalDelete { get; private set; }

        public long? GroupId { get; set; }

        public long? SubGroupId { get; set; }

        public List<PostAttachment> PostAttachments { get; private set; } = new();

        Post(string PostTitle, string PostContent, long? CategoryId, Guid UserId, string Tags, bool IsPrivate, bool IsDraft,
            long? GroupId, long? SubGroupId)
        { }// : base()
        //=> RecordThat(new PostCreated(Id, PostTitle, PostContent, CategoryId, UserId, Tags, IsPrivate, IsDraft,GroupId,SubGroupId));

        public static Post DefinePost(string PostTitle, string PostContent, long? CategoryId, Guid UserId,
            string Tags, bool IsPrivate, bool IsDraft, long? GroupId, long? SubGroupId)
        => new(PostTitle, PostContent, CategoryId, UserId, Tags, IsPrivate, IsDraft, GroupId, SubGroupId);

        public void AttachFile(string Title, long PostId, Guid UserId, string FileName, string FileType, string ContentType,
            long FileSize, string FilePath, byte[] File)
        { }
        //=> RecordThat(new PostFileAttached(id, Title, PostId, UserId, FileName, FileType, ContentType, string.Empty, FileSize, FilePath, File));

        public void DetachFile(long postAttachmentId) { }
        //=> RecordThat(new PostFileDeAttached(postAttachmentId));

        public void AddFile(long postAttachmentId, string FilePath)
        {
            PostAttachment postAttachment = PostAttachments.FirstOrDefault(c => c.Id == postAttachmentId);
            //if (postAttachment != null)
            //    RecordThat(new PostFileAdded(postAttachmentId, postAttachment.Title, postAttachment.PostId, postAttachment.UserId,
            //        postAttachment.FileName, postAttachment.FileType, postAttachment.ContentType, postAttachment.FileSystemName,
            //        postAttachment.FileSize, postAttachment.FilePath, postAttachment.File));
        }

        public void ChangePostProperties(long? CategoryId, string PostTitle, string PostContent, string tags, bool IsPrivate, bool IsDraft,
            long? GroupId, long? SubGroupId)
        { }
        //=> RecordThat(new PostPropertiesChanged(Id, CategoryId, UserId, PostTitle, PostContent, tags, IsPrivate, IsDraft,GroupId,SubGroupId));

        public void DeletePost() { }
        //=> RecordThat(new PostDeleted(Id));

        public void LogicalDeletePost(bool Archive) { }
        //=> RecordThat(new PostLogicalDeleted(Id, Archive));

        [Obsolete]
        Post()
        {
        }

        //void On(PostCreated e)
        //{
        //    PostTitle = e.PostTitle;
        //    PostContent = e.PostContent;
        //    CategoryId = e.CategoryId;
        //    UserId = e.UserId;
        //    IsPrivate = e.Private;
        //    IsDraft = e.Draft;
        //    GroupId = e.GroupId;
        //    SubGroupId = e.SubGroupId;
        //    LogicalDelete = e.LogicalDelete;
        //}

        //void On(PostFileAttached e)
        //{
        //    PostAttachments.Add(PostAttachment.AttachFile(e.Id, e.Title, Id, UserId, e.FileName, e.FileType,
        //        e.ContentType, e.FileSize, e.FilePath, e.File));
        //}

        //void On(PostFileDeAttached e)
        //{
        //    PostAttachment postAttachment = PostAttachments.FirstOrDefault(c => c.Id == e.Id);
        //    if (postAttachment != null)
        //        PostAttachments.Remove(postAttachment);
        //}

        //void On(PostFileAdded e)
        //{

        //}

        //void On(PostPropertiesChanged e)
        //{
        //    PostTitle = e.PostTitle;
        //    PostContent = e.PostContent;
        //    CategoryId = e.CategoryId;
        //    IsPrivate = e.Private;
        //    IsDraft = e.Draft;
        //    GroupId = e.GroupId;
        //    SubGroupId = e.SubGroupId;
        //}

        //void On(PostDeleted e)
        //{

        //}

        //void On(PostLogicalDeleted e)
        //{
        //    LogicalDelete = e.LogicalDelete;
        //}
    }
}
