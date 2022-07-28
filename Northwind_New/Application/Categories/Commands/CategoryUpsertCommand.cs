using CommandHandling.Abstractions;

namespace Application.Categories.Commands
{
    public record CategoryUpsertCommand(int? id, string Name, string Description, byte[] Picture) : Acommand(0)
    {
        public static CategoryUpsertCommand Create(int? id, string Name, string Description, byte[] Picture)
        {
            return new CategoryUpsertCommand(id, Name, Description, Picture);
        }
    }
}
