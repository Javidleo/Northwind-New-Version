using DomainModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataSource.Mapping
{
    public class ProductMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(e => e.ProductName)
                .IsRequired()
                .HasMaxLength(40);

            builder.Property(e => e.QuantityPerUnit).HasMaxLength(20);

            builder.Property(e => e.ReorderLevel).HasDefaultValueSql("((0))");

            builder.Property(e => e.UnitPrice)
                .HasColumnType("money")
                .HasDefaultValueSql("((0))");

            builder.Property(e => e.UnitsInStock).HasDefaultValueSql("((0))");

            builder.Property(e => e.UnitsOnOrder).HasDefaultValueSql("((0))");

        }
    }
}
