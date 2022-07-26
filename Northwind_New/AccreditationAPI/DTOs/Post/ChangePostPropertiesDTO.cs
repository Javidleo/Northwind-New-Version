using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace KnowledgeManagementAPI.DTOs.Post
{
    [Serializable]
    public class ChangePostPropertiesDTO
    {
        [Description("شناسه پست")]
        public long Id { get; set; }

        [Description("شناسه دسته بندی")]
        public long? CategoryId { get; set; }

        [Description("شناسه کاربر")]
        public Guid UserId { get; set; }

        [Description("عنوان پست")]
        public string PostTitle { get; set; }

        [Description("محتوای پست")]
        public string PostContent { get; set; }

        [Description("برچسب ها")]
        public string Tags { get; set; }

        [Description("پست خصوصی")]
        public bool IsPrivate { get; set; }

        [Description("پست پیش نویس")]
        public bool IsDraft { get; set; }

        [Description("شناسه گروه")]
        public long? GroupId { get; set; }

        [Description("شناسه زیرگروه")]
        public long? SubGroupId { get; set; }

        [Description("پیوست های جدید")]
        public List<FileInfo> NewFilesDetail { get; set; }

        [Description("پیوست هایی که می بایست حذف شوند")]
        public List<long> DeletedFiles { get; set; }
    }
}
