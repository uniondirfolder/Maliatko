using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.ViewModels
{
    public class ProductUserVM
    {
        public ProductUserVM()
        {
            ProductList = new LinkedList<Product>();
        }
        public ApplicationUser ApplicationUser { get; set; }
        public IEnumerable<Product> ProductList { get; set; }

        
    }
}
