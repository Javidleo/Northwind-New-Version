using System.Collections.Generic;

namespace DomainModel.Entities
{
    public class Territory
    {
        public Territory()
        => EmployeeTerritories = new HashSet<EmployeeTerritory>();

        public string Id { get; private set; }
        public string TerritoryDescription { get; private set; }
        public int RegionId { get; private set; }

        public Region Region { get; private set; }
        public ICollection<EmployeeTerritory> EmployeeTerritories { get; private set; }
    }
}
