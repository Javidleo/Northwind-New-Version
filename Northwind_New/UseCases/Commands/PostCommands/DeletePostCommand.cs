using CommandHandling.Abstractions;
using DataAccess;
using DomainModel;
using System.Threading.Tasks;
using UseCases.Common;
using UseCases.Common.Exceptions;

namespace UseCases.Commands.PostCommands
{
    public record DeletePostCommand(long Id) : Acommand(Id)
    {
        public static DeletePostCommand Create(long Id)
        => new(Id);


        public class DeletePostHandler : IHandleCommand<DeletePostCommand>
        {
            private readonly IWriteDbContext _context;
            public DeletePostHandler(IWriteDbContext context)
            => _context = context;

            public async Task Handle(DeletePostCommand command)
            {
                Post post = await _context.Posts.FindAsync(command.Id);
                if (post == null)
                    throw new NotFoundException("پست پیدا نشد.");

                if (post.IsDraft)
                {
                    post.DeletePost();
                    _context.Posts.Remove(post);
                }
                else
                {
                    post.LogicalDeletePost(true);
                    _context.Posts.Update(post);
                }
            }
        }

    }
}
