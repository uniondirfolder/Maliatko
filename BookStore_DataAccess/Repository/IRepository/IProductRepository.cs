using BookStore_Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BookStore_DataAccess.Repository.IRepository
{
    public interface IProductRepository : IBookStoreRepository<Product>
    {
        void Update(Product obj);

        IEnumerable<SelectListItem> GetAllDropdownList(string obj);
    }
}
