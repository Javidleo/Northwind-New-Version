using Application.Contracts;
using Application.Tests.Command;
using CommandHandling.Abstractions;
using DomainModel.Entities;
using System.Threading.Tasks;

namespace UseCases.CommandHandler
{

    public class CreateTestCommandHandler : IHandleCommand<CreateTestCommand>
    {
        private readonly ITestRepository _repository;
        public CreateTestCommandHandler(ITestRepository repository)
        {
            _repository = repository;
        }
        public Task Handle(CreateTestCommand command)
        {
            _repository.Add(new Test() { Name = command.name, Family = command.family, NationalCode = command.nationalCode });

            return Task.CompletedTask;
        }
    }

}
