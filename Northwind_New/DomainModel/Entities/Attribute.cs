using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    public class Attribute
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ProductAttribute> ProductAttributes { get; set; }
    }
}
