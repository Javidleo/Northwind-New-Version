using DomainModel;
using DomainModel.Document;
using DomainModel.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public interface IWriteDbContext
    {
        public void MarkAsAdded<Entity>(Entity entity) where Entity : class;
        public void MarkAsModified<Entity>(Entity entity) where Entity : class;
        public void MarkAsDeleted<Entity>(Entity entity) where Entity : class;

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
        public DbSet<Shipper> Shipper   { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<Territory> Territory { get; set; }
        public DbSet<DocumentView> DocumentView { get; set; }
        // new craeted
    }
}
