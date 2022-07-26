using CommandHandling.Abstractions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CommandHandling.MediatRAdopter
{
    public class MediatRPipelineBehaviorAdopter<Tcommand> : IPipelineBehavior<MediatRCommandEnvelope<Tcommand>, Unit> where Tcommand : Acommand
    {
        public ICommandStation<Tcommand> OurPipeLine { get; private set; }

        public MediatRPipelineBehaviorAdopter(ICommandStation<Tcommand> OurPipeLine)
        {
            this.OurPipeLine = OurPipeLine;
        }

        public async Task<Unit> Handle(MediatRCommandEnvelope<Tcommand> request, CancellationToken cancellationToken, RequestHandlerDelegate<Unit> next)
        {
            await OurPipeLine.Handle(request.Acommand, cancellationToken, () => next());
            return Unit.Value;
        }
    }
}
