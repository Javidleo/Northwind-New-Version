using QueryHandling.Abstractions;

namespace Application.Categories.Queries.GetAll
{
    public class CategoryListViewModel : IAmAViewModel
    {
        public IList<CategoryViewModel> Categories { get; set; }
        public int Count { get; set; }
    }
}