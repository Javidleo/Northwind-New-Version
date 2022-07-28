using Application.Common;
using Application.Contracts;
using CommandHandling.Abstractions;
using DomainModel.Entities;

namespace Application.Tests.Command
{
    public record CreateTestCommand(string name, string family, string nationalCode) : Acommand(0)
    {
        public static CreateTestCommand Create(string name, string family, string nationalCode)
        {
            return new CreateTestCommand(name, family, nationalCode);
        }
    }
}
