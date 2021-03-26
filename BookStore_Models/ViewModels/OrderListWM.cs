using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BookStore_Models.ViewModels
{
    public class OrderListWM
    {
        public IEnumerable<OrderHeader> OrderHeaderList { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }
        public string Status { get; set; }
    }
}
