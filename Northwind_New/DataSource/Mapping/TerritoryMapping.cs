using DomainModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataSource.Mapping
{
    public class TerritoryMapping : IEntityTypeConfiguration<Territory>
    {
        public void Configure(EntityTypeBuilder<Territory> builder)
        {
            builder.HasKey(i => i.Id).IsClustered(false);

            builder.Property(i => i.Id).HasMaxLength(20).ValueGeneratedNever();

            builder.Property(e => e.TerritoryDescription)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(d => d.Region)
                .WithMany(p => p.Territories)
                .HasForeignKey(d => d.RegionId);
        }
    }
}
