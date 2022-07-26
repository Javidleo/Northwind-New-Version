using System.Linq;

namespace ReadModels.Common
{
    public class PagingUtility
    {
        public static PagedViewModel<T> Paginate<T>(int pageNumber, int pageSize, IQueryable<T> allItems)
        {
            var info = new PagingInfo(pageNumber == 0 ? 1 : pageNumber
                               , pageSize == 0 ? 10 : pageSize
                               , allItems?.Count() ?? 0);

            var vm = new PagedViewModel<T>(info, allItems);
            return vm;
        }

        public static ListViewModel<T> NonPaginate<T>(IQueryable<T> AllItems)
        {
            return new ListViewModel<T>(AllItems);
        }
    }
}
