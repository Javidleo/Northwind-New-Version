using CommandHandling.Abstractions;

namespace Application.Categories.Commands.Delete
{
    public record DeleteCategoryCommand(int id) : Acommand(0)
    {
        public static DeleteCategoryCommand Create(int id) => new DeleteCategoryCommand(id);
    }
}
