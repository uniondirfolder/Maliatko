using BookStore_Models;

namespace BookStore_DataAccess.Repository.IRepository
{
    public interface ICategoryRepository : IBookStoreRepository<Category>
    {
        void Update(Category obj);
    }
}
