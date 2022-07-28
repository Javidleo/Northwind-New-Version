using System.Collections.Generic;

namespace DomainModel.Entities
{
    public class Region
    {
        public Region()
        => Territories = new HashSet<Territory>();

        public int Id { get; private set; }
        public string RegionDescription { get; private set; }

        public ICollection<Territory> Territories { get; private set; }
    }
}
