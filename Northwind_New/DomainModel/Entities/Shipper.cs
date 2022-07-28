using System.Collections.Generic;

namespace DomainModel.Entities
{
    public class Shipper
    {
        public Shipper()
        => Orders = new HashSet<Order>();

        public int Id { get; private set; }
        public string CompanyName { get; private set; }
        public string Phone { get; private set; }

        public ICollection<Order> Orders { get; private set; }
    }
}
