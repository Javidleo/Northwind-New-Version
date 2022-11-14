using DomainModel.Common;

namespace DomainModel.Entities
{
    public class OrderDetail : AuditableEntity
    {
        public decimal UnitPrice { get; private set; }
        public short Quantity { get; private set; }
        public float Discount { get; private set; }

        public int OrderId { get; private set; }
        public Order Order { get; private set; }

        public int ProductId { get; private set; }
        public Product Product { get; private set; }
    }
}
