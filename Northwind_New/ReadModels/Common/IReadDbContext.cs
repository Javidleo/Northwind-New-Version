using DomainModel;
using Microsoft.EntityFrameworkCore;
using ReadModels.DomainModel.Document;
using ReadModels.Queries.PostAttachmentQueries.GetPostAttachmentFile;
using ReadModels.Queries.PostQueries.GetPost;

namespace ReadModels.Common
{
    public interface IReadDbContext
    {
        public DbSet<PostViewModel> PostViewModels { get; set; }
        public DbSet<PostAttachmentViewModel> PostAttachmentViewModels { get; set; }
        public DbSet<DocumentView> DocumentView { get; set; }
    }
}
