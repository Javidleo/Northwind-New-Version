using CommandHandling.Abstractions;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace KnowledgeManagementAPI
{
    public class LoggingStation<TCommand> : ICommandStation<TCommand>
        where TCommand : Acommand
    {
        public Task Handle(TCommand command, CancellationToken cancellationToken, Func<Task> nextStation)
        {
            Console.WriteLine($"Handling command: {command.GetType()} with content: {JsonSerializer.Serialize(command)}");
            var result = nextStation();
            Console.WriteLine("Done.");
            return result;
        }
    }
}
