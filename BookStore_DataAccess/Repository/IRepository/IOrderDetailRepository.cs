using BookStore_Models;

namespace BookStore_DataAccess.Repository.IRepository
{
    public interface IOrderDetailRepository : IBookStoreRepository<OrderDetail>
    {
        void Update(OrderDetail obj);
    }
}
