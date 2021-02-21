using BookStore_DataAccess.Repository.IRepository;
using BookStore_Models;
using BookStore_Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace BookStore_DataAccess.Repository
{
    public class InquiryHeaderRepository : BookStoreRepository<InquiryHeader>, IInquiryHeaderRepository
    {
        private readonly ApplicationDbContext _db;

        public InquiryHeaderRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
  

        public void Update(InquiryHeader obj)
        {
            _db.InquiryHeaders.Update(obj);
        }
    }
}
