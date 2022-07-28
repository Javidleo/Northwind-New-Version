using CommandHandling.Abstractions;

namespace Application.Test.Command
{
    public class TestCommandHandler : IHandleCommand<TestCommand>
    {
        public Task Handle(TestCommand command)
        {
            return Task.CompletedTask;
        }
    }
}
