using MediatR;
using QueryHandling.Abstractions;

namespace QueryHandling.MediatRAdopter
{
    public class MediatRQueryEnvelope<TQuery, TviewModel> : IRequest<TviewModel> where TQuery : Query<TviewModel> where TviewModel : IAmAViewModel
    {
        public TQuery Query { get; private set; }

        public MediatRQueryEnvelope(TQuery Query)
        {
            this.Query = Query;
        }
    }
}
