using CommandHandling.Abstractions;
using CustomException.Exceptions;

namespace Application.Categories.Commands
{
    public record UpsertCategoryCommand(int? id, string Name, string Description, byte[] Picture) : Acommand(0)
    {
        public static UpsertCategoryCommand Create(int? id, string Name, string Description, byte[] Picture)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new NotAcceptableException("name is empty");

            return new UpsertCategoryCommand(id, Name, Description, Picture);
        }
    }
}
