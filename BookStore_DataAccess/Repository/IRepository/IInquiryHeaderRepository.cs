using BookStore_Models;

namespace BookStore_DataAccess.Repository.IRepository
{
    public interface IInquiryHeaderRepository : IBookStoreRepository<InquiryHeader>
    {
        void Update(InquiryHeader obj);
    }
}
