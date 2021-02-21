using BookStore_Models;

namespace BookStore_DataAccess.Repository.IRepository
{
    public interface IInquiryDetailRepository : IBookStoreRepository<InquiryDetail>
    {
        void Update(InquiryDetail obj);
    }
}
