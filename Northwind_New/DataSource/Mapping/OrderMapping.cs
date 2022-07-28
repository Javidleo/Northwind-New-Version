using DomainModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSource.Mapping
{
    public class OrderMapping : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {

            builder.HasKey(i => i.Id);
            builder.Property(e => e.CustomerId)
               .HasColumnName("CustomerID")
               .HasMaxLength(5);

            builder.Property(e => e.Freight)
                .HasColumnType("money")
                .HasDefaultValueSql("((0))");

            builder.Property(e => e.OrderDate).HasColumnType("datetime");

            builder.Property(e => e.RequiredDate).HasColumnType("datetime");

            builder.Property(e => e.ShipAddress).HasMaxLength(60);

            builder.Property(e => e.ShipCity).HasMaxLength(15);

            builder.Property(e => e.ShipCountry).HasMaxLength(15);

            builder.Property(e => e.ShipName).HasMaxLength(40);

            builder.Property(e => e.ShipPostalCode).HasMaxLength(10);

            builder.Property(e => e.ShipRegion).HasMaxLength(15);

            builder.Property(e => e.ShippedDate).HasColumnType("datetime");

            builder.HasOne(d => d.Shipper)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.ShipVia);
        }
    }
}
