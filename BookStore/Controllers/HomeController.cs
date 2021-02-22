using BookStore_DataAccess;
using BookStore_DataAccess.Repository.IRepository;
using BookStore_Models;
using BookStore_Models.ViewModels;
using BookStore_Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _prodRepo;
        private readonly ICategoryRepository _catgRepo;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, IProductRepository prodRepo, ICategoryRepository catgRepo)
        {
            _logger = logger;
            _prodRepo = prodRepo;
            _catgRepo = catgRepo;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM() {
                Products = _prodRepo.GetAll(includeProperties: "Category,ApplicationType"),
                Categories = _catgRepo.GetAll()
            };
            return View(homeVM);
        }

        public IActionResult Details(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null &&
                HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }


            DetailsVM DetailsVM = new DetailsVM()
            {
                Product = _prodRepo.FirstOrDefault(x=>x.Id==id,includeProperties: "Category,ApplicationType"),                
                ExistsInCart = false
            };

            foreach (var item in shoppingCartList)
            {
                if(item.ProductId == id) 
                {
                    DetailsVM.ExistsInCart = true;
                }
            }


            return View(DetailsVM);
        }

        [HttpPost,ActionName("Details")]
        public IActionResult DetailsPost(int id, DetailsVM detailsVM)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null &&
                HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            shoppingCartList.Add(new ShoppingCart { ProductId = id, SqFt = detailsVM.Product.TempSqFt});
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null &&
                HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            var itemToRemove = shoppingCartList.SingleOrDefault(l => l.ProductId == id);
            if (itemToRemove != null) { shoppingCartList.Remove(itemToRemove); }

            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
