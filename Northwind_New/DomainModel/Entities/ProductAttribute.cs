
namespace DomainModel.Entities
{
    public class ProductAttribute
    {
        public int Id { get; set; }
        public bool IsSpecification { get; set; }
        public double PriceAddAmount { get; set; }
        public int PriceAddPercent { get; set; }
        public int Count { get; set; }

        public int AttributeId { get; set; }
        public virtual Attribute Attribute { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int ValueId { get; set; }
        public virtual AttributeValue AttributeValue { get; set; }
    }
}
