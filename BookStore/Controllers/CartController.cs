using BookStore_DataAccess;
using BookStore_DataAccess.Repository.IRepository;
using BookStore_Models;
using BookStore_Models.ViewModels;
using BookStore_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnviroment;
        private readonly IEmailSender _emailSender;
        private readonly IApplicationUserRepository _userRepo;
        private readonly IProductRepository _productRepo;
        private readonly IInquiryHeaderRepository _headerRepo;
        private readonly IInquiryDetailRepository _detailRepo;

        [BindProperty]
        public ProductUserVM ProductUserVM { get; set; }
        public CartController(IWebHostEnvironment webHostEnviroment, 
            IEmailSender emailSender,
            IApplicationUserRepository userRepo,
            IProductRepository productRepo,
            IInquiryHeaderRepository headerRepo,
            IInquiryDetailRepository detailRepo)
        {
            _webHostEnviroment = webHostEnviroment;
            _emailSender = emailSender;
            _userRepo = userRepo;
            _productRepo = productRepo;
            _headerRepo = headerRepo;
            _detailRepo = detailRepo;
        }

        public IActionResult Index()
        {
            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null &&
                HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                //session exsits
                shoppingCarts = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            List<int> prodInCart = shoppingCarts.Select(l => l.ProductId).ToList();
            IEnumerable<Product> prodListTemp = _productRepo.GetAll(l => prodInCart.Contains(l.Id));
            IList<Product> prodList = new List<Product>();

            foreach (var cartObj in shoppingCarts)
            {
                Product prodTemp = prodListTemp.FirstOrDefault(x => x.Id == cartObj.ProductId);
                prodTemp.TempSqFt = cartObj.SqFt;
                prodList.Add(prodTemp);
            }
            return View(prodList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost(IEnumerable<Product> productList)
        {
            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            foreach (Product prod in productList)
            {
                shoppingCarts.Add(new ShoppingCart { ProductId = prod.Id, SqFt = prod.TempSqFt });
            }

            HttpContext.Session.Set(WC.SessionCart, shoppingCarts);

            return RedirectToAction(nameof(Summary));
        }
        public IActionResult Summary() 
        {
            ApplicationUser applicationUser;

            if (User.IsInRole(WC.AdminRole)) 
            {
                //cart has been loaded using an inquiry
                if (HttpContext.Session.Get<int>(WC.SessionInquiryId)!=0)
                {
                    InquiryHeader inquiryHeader = _headerRepo.FirstOrDefault(x => x.Id == HttpContext.Session.Get<int>(WC.SessionInquiryId));
                    applicationUser = new ApplicationUser()
                    {
                        Email = inquiryHeader.Email,
                        FullName = inquiryHeader.FullName,
                        PhoneNumber = inquiryHeader.PhoneNumber
                    };
                }
                else
                {
                    applicationUser = new ApplicationUser();
                }
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                //var userId = User.FindFirstValue(ClaimTypes.Name);
                applicationUser = _userRepo.FirstOrDefault(x => x.Id == claim.Value);
            }


            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null &&
                HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                //session exsits
                shoppingCarts = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            List<int> prodInCart = shoppingCarts.Select(l => l.ProductId).ToList();
            IEnumerable<Product> prodList = _productRepo.GetAll(l => prodInCart.Contains(l.Id));

            ProductUserVM = new ProductUserVM()
            {
                ApplicationUser = applicationUser,
            };

            foreach (var cartObj in shoppingCarts)
            {
                Product prodTemp = _productRepo.FirstOrDefault(x => x.Id == cartObj.ProductId);
                prodTemp.TempSqFt = cartObj.SqFt;
                ProductUserVM.ProductList.Add(prodTemp);
            }

            return View(ProductUserVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(ProductUserVM ProductUserVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var PathToTemplate = _webHostEnviroment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                + "templates" + Path.DirectorySeparatorChar.ToString() +
                "Inquiry.html";

            var subject = "New Inquiry";
            string HtmlBody = "";
            using (StreamReader sr = System.IO.File.OpenText(PathToTemplate))
            {
                HtmlBody = sr.ReadToEnd();
            }

            StringBuilder productListSB = new StringBuilder();
            foreach (var prod in ProductUserVM.ProductList)
            {
                productListSB.Append($" - Name: {prod.Name} <span style='font-size:14px;'> (ID: {prod.Id})</span><br />");
            }

            string messageBody = string.Format(HtmlBody,
                ProductUserVM.ApplicationUser.FullName,
                ProductUserVM.ApplicationUser.Email,
                ProductUserVM.ApplicationUser.PhoneNumber,
                productListSB.ToString());

            await _emailSender.SendEmailAsync(WC.EmailAdmin, subject, messageBody);

            InquiryHeader inquiryHeader = new InquiryHeader()
            {
                ApplicationUserId = claim.Value,
                FullName = ProductUserVM.ApplicationUser.FullName,
                Email = ProductUserVM.ApplicationUser.Email,
                PhoneNumber = ProductUserVM.ApplicationUser.PhoneNumber,
                InquiryDate = DateTime.Now
            };

            _headerRepo.Add(inquiryHeader);
            _headerRepo.Save();

            foreach (var prod in ProductUserVM.ProductList)
            {
                InquiryDetail inquiryDetail = new InquiryDetail() 
                {
                    InquiryHeaderId = inquiryHeader.Id,
                    ProductId = prod.Id
                };
                _detailRepo.Add(inquiryDetail);              
            }           
            _detailRepo.Save();

            TempData[WC.Success] = "Inquiry summury successfully";
            return RedirectToAction(nameof(InquiryConfirmation));
        }
        public IActionResult InquiryConfirmation() 
        {
            HttpContext.Session.Clear();
            return View();
        }

        public IActionResult Remove(int id)
        {
            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null &&
                HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                //session exsits
                shoppingCarts = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            shoppingCarts.Remove(shoppingCarts.FirstOrDefault(l => l.ProductId == id));
            HttpContext.Session.Set(WC.SessionCart, shoppingCarts);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCart(IEnumerable<Product> productList) 
        {
            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            foreach (Product prod in productList)
            {
                shoppingCarts.Add(new ShoppingCart { ProductId = prod.Id, SqFt = prod.TempSqFt });
            }

            HttpContext.Session.Set(WC.SessionCart, shoppingCarts);
            return RedirectToAction(nameof(Index));
        }
    }
}
