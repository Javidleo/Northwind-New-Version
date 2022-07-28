using Application.Contracts;
using CommandHandling.Abstractions;
using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests.Command
{
    public class TestHandler : IHandleCommand<CreateTestCommand>
    {
        private readonly ITestRepository _repository;
        public TestHandler(ITestRepository repository)
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
