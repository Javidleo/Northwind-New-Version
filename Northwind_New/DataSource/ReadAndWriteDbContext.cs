using Application.Common;
using DataAccess;
using DataSource.Mapping;
using DomainModel;
using DomainModel.Common;
using DomainModel.Document;
using DomainModel.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DataSource
{
    public class ReadAndWriteDbContext : DbContext, IWriteDbContext, IUnitOfWork, IReadDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        public ReadAndWriteDbContext(DbContextOptions<ReadAndWriteDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public ReadAndWriteDbContext(DbContextOptions<ReadAndWriteDbContext> options,
                                        ICurrentUserService currentUserService) : base(options)
        {
            Database.EnsureCreated();
            _currentUserService = currentUserService;
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

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReadAndWriteDbContext).Assembly);
            // new Models
        }
        // this SaveChanges Method working asynchronsly and get userId from User Calims;
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach(var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State) 
                {
                    case EntityState.Added: // if entity state is added take userId from claim and use it as createBy
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.Created = Calender.CurrentDateWithTime;
                        break;

                    case EntityState.Modified: // if entity state is modified take userId from claim and use it as modifiedby
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.LastModified = Calender.CurrentDateWithTime;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }


        /////////////////////////////////////////////////////////Aggregates and ChildEntities
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostAttachment> PostAttachments { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<EmployeeTerritory> EmployeeTerritory { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Region> Region { get; set; }
        public DbSet<Shipper> Shipper { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<Territory> Territory { get; set; }
        public DbSet<DocumentView> DocumentView { get; set; }
        //DbSet<SubGroupViewModel> SubGroupViewModel { get; set; }
    }
}
