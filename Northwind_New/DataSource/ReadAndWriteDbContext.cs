using DataSource.Mapping;
using DomainModel;
using Microsoft.EntityFrameworkCore;
using ReadModels.Common;
using ReadModels.DomainModel.Document;
using ReadModels.Queries.PostAttachmentQueries.GetPostAttachmentFile;
using ReadModels.Queries.PostQueries.GetPost;
using UseCases.Common;

namespace DataSource
{
    public class ReadAndWriteDbContext : DbContext, IWriteDbContext, IUnitOfWork, IReadDbContext
    {
        public ReadAndWriteDbContext(DbContextOptions<ReadAndWriteDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public void MarkAsAdded<Entity>(Entity entity) where Entity : class
        => Entry(entity).State = EntityState.Added;

        public void MarkAsModified<Entity>(Entity entity) where Entity : class
        => Entry(entity).State = EntityState.Modified;

        public void MarkAsDeleted<Entity>(Entity entity) where Entity : class
        => Entry(entity).State = EntityState.Deleted;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PostMapping());
            modelBuilder.ApplyConfiguration(new PostAttachmentMapping());

            // new Models

        }

        /////////////////////////////////////////////////////////Aggregates and ChildEntities
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostAttachment> PostAttachments { get; set; }

        /////////////////////////////////////////////////////////ReadModels
        public DbSet<PostViewModel> PostViewModels { get; set; }
        public DbSet<PostAttachmentViewModel> PostAttachmentViewModels { get; set; }
        /////////////////////////////////////////////////////////Views
        public DbSet<DocumentView> DocumentView { get; set; }
        //DbSet<SubGroupViewModel> SubGroupViewModel { get; set; }
    }
}
