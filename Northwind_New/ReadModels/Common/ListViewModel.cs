using QueryHandling.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace ReadModels.Common
{
    public class ListViewModel<T> : IAmAViewModel
    {
        public IEnumerable<T> Data { get; set; }

        public ListViewModel(IQueryable<T> AllItems)
        {
            Data = AllItems;
        }
    }
}
