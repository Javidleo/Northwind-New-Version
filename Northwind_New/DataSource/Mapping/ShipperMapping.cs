using DomainModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataSource.Mapping
{
    public class ShipperMapping : IEntityTypeConfiguration<Shipper>
    {
        public void Configure(EntityTypeBuilder<Shipper> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(e => e.CompanyName)
                .IsRequired()
                .HasMaxLength(40);

            builder.Property(e => e.Phone).HasMaxLength(24);

            builder.HasMany(i => i.Orders).WithOne(i => i.Shipper).HasForeignKey(i => i.ShipVia);
        }
    }
}
