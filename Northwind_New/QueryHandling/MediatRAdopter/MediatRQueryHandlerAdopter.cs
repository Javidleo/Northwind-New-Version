using MediatR;
using QueryHandling.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace QueryHandling.MediatRAdopter
{
    public class MediatRQueryHandlerAdopter<TQuery, TViewModel> : IRequestHandler<MediatRQueryEnvelope<TQuery, TViewModel>, TViewModel>
        where TQuery : Query<TViewModel> where TViewModel : IAmAViewModel
    {
        readonly IHandleQuery<TQuery, TViewModel> ourHandler;

        public MediatRQueryHandlerAdopter(IHandleQuery<TQuery, TViewModel> ourHandler)
        {
            this.ourHandler = ourHandler;
        }
        public async Task<TViewModel> Handle(MediatRQueryEnvelope<TQuery, TViewModel> request, CancellationToken cancellationToken)
        {
            return await ourHandler.Handle(request.Query);
        }
    }
}
