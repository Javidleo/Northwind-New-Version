using CommandHandling.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandHandling.MediatRAdopter
{
    public class MediatRCommandBusAdopter : ICommandBus
    {
        public IMediator mediator { get; set; }

        public MediatRCommandBusAdopter(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public Task Send<Tcommand>(Tcommand command) where Tcommand : Acommand
        {
            return mediator.Send(new MediatRCommandEnvelope<Tcommand>(command));
        }
    }
}
