using BookStore_DataAccess.Repository.IRepository;
using BookStore_Utility.BrainTree;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderHeaderRepository _orderHeaderRepo;
        private readonly IOrderDetailRepository _orderDetailRepo;
        private readonly IBrainTreeGate _brain;

        public OrderController(
            IOrderHeaderRepository orderHeaderRepo,
            IOrderDetailRepository orderDetailRepo,
            IBrainTreeGate brain)
        {
            _orderHeaderRepo = orderHeaderRepo;
            _orderDetailRepo = orderDetailRepo;
            _brain = brain;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
