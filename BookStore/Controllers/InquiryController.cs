using BookStore_DataAccess.Repository.IRepository;
using BookStore_Models;
using BookStore_Models.ViewModels;
using BookStore_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BookStore.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class InquiryController : Controller
    {
        private readonly IInquiryHeaderRepository _inquiryHeaderRepository;
        private readonly IInquiryDetailRepository _inquiryDetailRepository;
        
        [BindProperty]
        public InquiryVM InquiryVM { get; set; }

        public InquiryController(IInquiryHeaderRepository inquiryHeaderRepository, 
            IInquiryDetailRepository inquiryDetailRepository)
        {
            _inquiryHeaderRepository = inquiryHeaderRepository;
            _inquiryDetailRepository = inquiryDetailRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id) 
        {
            InquiryVM = new InquiryVM()
            {
                InquiryHeader = _inquiryHeaderRepository.FirstOrDefault(x => x.Id == id),
                InquiryDetails = _inquiryDetailRepository.GetAll(x => x.InquiryHeaderId == id, includeProperties: "Product")
            };
            return View(InquiryVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details()
        {
            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            InquiryVM.InquiryDetails = _inquiryDetailRepository.GetAll(x => x.InquiryHeaderId == InquiryVM.InquiryHeader.Id);

            foreach (var detail in InquiryVM.InquiryDetails)
            {
                ShoppingCart shoppingCart = new ShoppingCart()
                {
                    ProductId = detail.ProductId
                };
                shoppingCarts.Add(shoppingCart);
            }
            HttpContext.Session.Clear();
            HttpContext.Session.Set(WC.SessionCart, shoppingCarts);
            HttpContext.Session.Set(WC.SessionInquiryId, InquiryVM.InquiryHeader.Id);
            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public IActionResult Delete()
        {
            InquiryHeader inquiryHeader = _inquiryHeaderRepository.FirstOrDefault(x => x.Id == InquiryVM.InquiryHeader.Id);
            IEnumerable<InquiryDetail> inquiryDetails = _inquiryDetailRepository.GetAll(x => x.InquiryHeaderId == InquiryVM.InquiryHeader.Id);

            _inquiryDetailRepository.RemoveRange(inquiryDetails);
            _inquiryHeaderRepository.Remove(inquiryHeader);
            _inquiryHeaderRepository.Save();
            TempData[WC.Success] = "Inquiry deleted successfully";
            return RedirectToAction(nameof(Index));
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetInquiryList() 
        {
            return Json(new { data = _inquiryHeaderRepository.GetAll() });
        }

        #endregion
    }
}
