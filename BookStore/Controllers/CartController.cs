using BookStore.Data;
using BookStore.Models;
using BookStore.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            if(HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart)!=null &&
                HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCarts = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            List<int> prodInCart = shoppingCarts.Select(l => l.ProductId).ToList();
            IEnumerable<Product> prodList = _db.Products.Where(l => prodInCart.Contains(l.Id));
            return View();
        }
    }
}
