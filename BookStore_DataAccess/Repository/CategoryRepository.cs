using BookStore_DataAccess.Repository.IRepository;
using BookStore_Models;
using System.Linq;

namespace BookStore_DataAccess.Repository
{
    public class CategoryRepository : BookStoreRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public void Update(Category obj)
        {
            var objFromDb = _db.Categories.FirstOrDefault(l => l.Id == obj.Id);
            if (objFromDb!=null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.DisplayOrder = obj.DisplayOrder;
            }
        }
    }
}
