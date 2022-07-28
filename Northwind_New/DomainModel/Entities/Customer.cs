using System.Collections.Generic;

namespace DomainModel.Entities
{
    public class Customer
    {
        public Customer()
        => Orders = new HashSet<Order>();

        public string Id { get; private set; }
        public string CompanyName { get; private set; }
        public string ContactName { get; private set; }
        public string ContactTitle { get; private set; }
        public string Address { get; private set; }
        public string City { get; private set; }
        public string Region { get; private set; }
        public string PostalCode { get; private set; }
        public string Country { get; private set; }
        public string Phone { get; private set; }
        public string Fax { get; private set; }

        public ICollection<Order> Orders { get; private set; }
    }
}
