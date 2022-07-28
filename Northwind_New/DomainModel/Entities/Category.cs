using System.Collections.Generic;

namespace DomainModel.Entities
{
    public class Category
    {
        public Category()
        => Products = new HashSet<Product>();

        public int Id { get; private set; }
        public string CategoryName { get; private  set; }
        public string Description { get; private set; }
        public byte[] Picture { get; private set; }

        public virtual ICollection<Product> Products { get; private set; }
    }
}
