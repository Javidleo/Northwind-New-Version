using CommandHandling.Abstractions;
using MediatR;

namespace CommandHandling.MediatRAdopter
{
    public class MediatRCommandEnvelope<Tcommand> : IRequest where Tcommand : Acommand
    {
        public MediatRCommandEnvelope(Tcommand command)
        {
            Acommand = command;
        }

        public Tcommand Acommand { get; private set; }
    }
}
