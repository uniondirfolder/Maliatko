using BookStore_DataAccess.Repository.IRepository;
using BookStore_Models;
using System.Linq;

namespace BookStore_DataAccess.Repository
{
    public class ApplicationTypeRepository : BookStoreRepository<ApplicationType>, IApplicationTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public ApplicationTypeRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public void Update(ApplicationType obj)
        {
            var objFromDb = base.FirstOrDefault(l => l.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
            }
        }
    }
}
