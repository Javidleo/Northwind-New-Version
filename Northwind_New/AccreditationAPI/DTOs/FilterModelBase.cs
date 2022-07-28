namespace AccreditationAPI.DTOs
{
    public abstract class FilterModelBase
    {
        const int maxPageSize = 200;

        public int PageNumber { get; set; } = 1;

        private int? _pageSize;
        public int PageSize
        {
            get
            {
                return _pageSize ?? 0;
            }
            set
            {
                _pageSize = value > maxPageSize ? maxPageSize : value;
            }
        }

        public string SortOrder { get; set; }
    }
}
