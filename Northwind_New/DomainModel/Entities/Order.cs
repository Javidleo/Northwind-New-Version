using DomainModel.Common;
using System;
using System.Collections.Generic;

namespace DomainModel.Entities
{
    public class Order : AuditableEntity
    {
        public Order()
        => OrderDetails = new HashSet<OrderDetail>();

        public int Id { get; private set; }
        public DateTime? OrderDate { get; private set; }
        public DateTime? RequiredDate { get; private set; }
        public DateTime? ShippedDate { get; private set; }

        public decimal? Freight { get; private set; }
        public string ShipName { get; private set; }
        public string ShipAddress { get; private set; }
        public string ShipCity { get; private set; }
        public string ShipRegion { get; private set; }
        public string ShipPostalCode { get; private set; }
        public string ShipCountry { get; private set; }

        public string CustomerId { get; private set; }
        public Customer Customer { get; set; }

        public int? EmployeeId { get; private set; }
        public Employee Employee { get; set; }

        public int? ShipVia { get; private set; }
        public Shipper Shipper { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; private set; }
    }
}
