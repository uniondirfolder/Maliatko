using BookStore_DataAccess.Repository.IRepository;
using BookStore_Models;
using System.Linq;

namespace BookStore_DataAccess.Repository
{
    public class ApplicationUserRepository : BookStoreRepository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
    }
}
