using DomainModel;
using Microsoft.EntityFrameworkCore;
using ReadModels.DomainModel.Document;

namespace UseCases.Common
{
    public interface IWriteDbContext
    {
        public void MarkAsAdded<Entity>(Entity entity) where Entity : class;
        public void MarkAsModified<Entity>(Entity entity) where Entity : class;
        public void MarkAsDeleted<Entity>(Entity entity) where Entity : class;

        public DbSet<Post> Posts { get; set; }
        public DbSet<PostAttachment> PostAttachments { get; set; }
        public DbSet<DocumentView> DocumentView { get; set; }
        // new craeted
    }
}
