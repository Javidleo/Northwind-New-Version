using DomainModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataSource.Mapping
{
    public class CustomerMapping : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                    .HasMaxLength(5)
                    .ValueGeneratedNever();

            builder.Property(e => e.Address).HasMaxLength(60);

            builder.Property(e => e.City).HasMaxLength(15);

            builder.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(40);

            builder.Property(e => e.ContactName).HasMaxLength(30);

            builder.Property(e => e.ContactTitle).HasMaxLength(30);

            builder.Property(e => e.Country).HasMaxLength(15);

            builder.Property(e => e.Fax).HasMaxLength(24);

            builder.Property(e => e.PhoneNumber).HasMaxLength(24);

            builder.Property(e => e.PostalCode).HasMaxLength(10);

            builder.Property(e => e.Region).HasMaxLength(15);

            builder.HasMany(i => i.Orders).WithOne(i => i.Customer).HasForeignKey(i => i.CustomerId);
        }
    }
}
