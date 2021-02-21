using BookStore_Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BookStore_DataAccess.Repository.IRepository
{
    public interface IInquiryHeaderRepository : IBookStoreRepository<InquiryHeader>
    {
        void Update(InquiryHeader obj);
    }
}
