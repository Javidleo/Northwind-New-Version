using System.Threading.Tasks;

namespace QueryHandling.Abstractions
{
    public interface IHandleQuery<in TQuery, TviewModel> where TQuery : Query<TviewModel> where TviewModel : IAmAViewModel
    {
        public Task<TviewModel> Handle(TQuery query);
    }
}
