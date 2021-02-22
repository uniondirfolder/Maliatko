using BookStore_DataAccess.Repository.IRepository;
using BookStore_Models;

namespace BookStore_DataAccess.Repository
{
    public class OrderDetailRepository : BookStoreRepository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderDetailRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public void Update(OrderDetail obj)
        {
            _db.OrderDetails.Update(obj);
        }
    }
}
