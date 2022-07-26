using CommandHandling.Abstractions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CommandHandling.MediatRAdopter
{
    public class MediatRHandlerAdopte<Tcommand> : IRequestHandler<MediatRCommandEnvelope<Tcommand>, Unit> where Tcommand : Acommand
    {
        public IHandleCommand<Tcommand> OurHandler { get; set; }

        public MediatRHandlerAdopte(IHandleCommand<Tcommand> OurHandler)
        {
            this.OurHandler = OurHandler;
        }

        public async Task<Unit> Handle(MediatRCommandEnvelope<Tcommand> request, CancellationToken cancellationToken)
        {
            await OurHandler.Handle(request.Acommand);
            return new Unit();
        }
    }
}
