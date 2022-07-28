using DomainModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataSource.Mapping
{
    public class CategoryMapping : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(e => e.CategoryName)
               .IsRequired()
               .HasMaxLength(15);

            builder.Property(e => e.Description).HasColumnType("ntext");

            builder.Property(e => e.Picture).HasColumnType("image");
            builder.HasMany(i => i.Products).WithOne(i => i.Category);
        }
    }
}
