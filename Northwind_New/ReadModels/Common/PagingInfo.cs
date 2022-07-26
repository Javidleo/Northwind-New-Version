using System;

namespace ReadModels.Common
{
    public class PagingInfo
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPageCount { get; }

        public PagingInfo(int number, int size, int totalCount)
        {
            PageNumber = number;
            PageSize = size;
            TotalCount = totalCount;
            TotalPageCount = size != 0 ? (int)Math.Ceiling(totalCount / (double)size) : 1;
        }
    }
}