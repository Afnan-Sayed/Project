using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_API.DataAccess.Entities.Inventory
{
    public class Category
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        // Relationship: One Category has many Products
        // using "ICollection" is standard for "Many" side relationships in EF Core
        public required ICollection<Product> Products { get; set; }
    }
}
