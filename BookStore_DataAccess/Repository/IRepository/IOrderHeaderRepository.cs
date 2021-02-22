using BookStore_Models;

namespace BookStore_DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IBookStoreRepository<OrderHeader>
    {
        void Update(OrderHeader obj);
    }
}
