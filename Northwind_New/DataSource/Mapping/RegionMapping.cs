using DomainModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataSource.Mapping
{
    public class RegionMapping : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.RegionDescription)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
