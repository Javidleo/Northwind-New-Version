using System.Collections.Generic;

namespace DomainModel.Entities
{
    public class AttributeValue
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ProductAttribute> ProductAttributes { get; set; }
    }
}