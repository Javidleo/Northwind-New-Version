using MediatR;
using QueryHandling.Abstractions;
using System.Threading.Tasks;

namespace QueryHandling.MediatRAdopter
{
    internal class MediatRQueryBusAdopter : IQueryBus
    {
        readonly IMediator mediatr;

        public MediatRQueryBusAdopter(IMediator mediatr)
        {
            this.mediatr = mediatr;
        }
        public async Task<TviewModel> Send<TviewModel, TQuery>(TQuery Query)
            where TviewModel : IAmAViewModel
            where TQuery : Query<TviewModel>
        {
            return await mediatr.Send(new MediatRQueryEnvelope<TQuery, TviewModel>(Query));
        }
    }
}
