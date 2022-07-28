using DomainModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataSource.Mapping
{
    public class OrderDetailMapping : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(e => new { e.OrderId, e.ProductId });

            builder.ToTable("Order Details");

            builder.Property(e => e.OrderId).HasColumnName("OrderID");

            builder.Property(e => e.ProductId).HasColumnName("ProductID");

            builder.Property(e => e.Quantity).HasDefaultValueSql("((1))");

            builder.Property(e => e.UnitPrice).HasColumnType("money");

            builder.HasOne(i => i.Order).WithMany(i => i.OrderDetails).HasForeignKey(i => i.OrderId);
            builder.HasOne(i => i.Product).WithMany(i => i.OrderDetails).HasForeignKey(i => i.ProductId);
        }
    }
}
