using CommandHandling.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Test.Command
{
    public record TestCommand() : Acommand(0);
}
