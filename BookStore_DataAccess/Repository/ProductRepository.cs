using BookStore_DataAccess.Repository.IRepository;
using BookStore_Models;
using BookStore_Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace BookStore_DataAccess.Repository
{
    public class ProductRepository : BookStoreRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetAllDropdownList(string obj)
        {
            if (obj == WC.CategoryName) 
            {
                return _db.Categories.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                
            }
            if(obj == WC.ApplicationTypeName) 
            {
                return _db.ApplicationTypes.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            return null;
        }

        public void Update(Product obj)
        {
            _db.Products.Update(obj);
        }
    }
}
