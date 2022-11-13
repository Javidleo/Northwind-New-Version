using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Entities
{
    public class Category
    {
        public Category()
        => Products = new HashSet<Product>();

        public int Id { get; private set; }
        public string CategoryName { get; private set; }
        public string Description { get; private set; }
        public byte[] Picture { get; private set; }

        [ForeignKey("ParentId")]
        public int? ParentId { get; set; }

        public virtual ICollection<Product> Products { get; private set; }

        public Category(string name, string description, byte[] picture)
        {
            CategoryName = name;
            Description = description;
            Picture = picture;
        }

        public static Category Create(string name, string description, byte[] picture)
        => new(name, description, picture);

        public void ChangeProperties(string name, string description, byte[] picture)
        {
            CategoryName = name;
            Description = description;
            Picture = picture;
        }

        public void Modify(string name, string description, byte[] picture)
        {
            CategoryName = name;
            Description = description;
            Picture = picture;
        }
    }
}
