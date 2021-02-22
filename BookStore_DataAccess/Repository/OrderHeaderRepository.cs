using BookStore_DataAccess.Repository.IRepository;
using BookStore_Models;

namespace BookStore_DataAccess.Repository
{
    public class OrderHeaderRepository : BookStoreRepository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderHeaderRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
  

        public void Update(OrderHeader obj)
        {
            _db.OrderHeaders.Update(obj);
        }
    }
}
