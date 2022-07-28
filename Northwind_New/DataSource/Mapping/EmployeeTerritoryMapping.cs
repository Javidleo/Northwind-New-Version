using DomainModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataSource.Mapping
{
    public class EmployeeTerritoryMapping : IEntityTypeConfiguration<EmployeeTerritory>
    {
        public void Configure(EntityTypeBuilder<EmployeeTerritory> builder)
        {
            builder.HasKey(i => new { i.EmployeeId, i.TerritoryId });

            builder.HasOne(i => i.Employee).WithMany(i => i.EmployeeTerritories);
            builder.HasOne(i => i.Territory).WithMany(i => i.EmployeeTerritories);
        }
    }
}
