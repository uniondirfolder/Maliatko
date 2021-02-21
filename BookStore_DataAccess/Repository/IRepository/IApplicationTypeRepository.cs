using BookStore_Models;

namespace BookStore_DataAccess.Repository.IRepository
{
    public interface IApplicationTypeRepository : IBookStoreRepository<ApplicationType>
    {
        void Update(ApplicationType obj);
    }
}
