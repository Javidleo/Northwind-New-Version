using System.ComponentModel;

namespace AccreditationAPI.DTOs.Post
{
    public class GetPostDTO : FilterModelBase
    {
        [Description("عنوان پست")]
        public string PostTitle { get; set; }

        [Description("محتوای پست")]
        public string PostContent { get; set; }

        [Description("شناسه دسته بندی")]
        public long? CategoryID { get; set; }

        [Description("شناسه گروه")]
        public long? GroupId { get; set; }

        [Description("شناسه زیرگروه")]
        public long? SubGroupId { get; set; }

        [Description("برچسب ها")]
        public string Tags { get; set; }

        [Description("پست خصوصی")]
        public bool? Private { get; set; }

        [Description("پست پیش نویس")]
        public bool? Draft { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }
    }
}
