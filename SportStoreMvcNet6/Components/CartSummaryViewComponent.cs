using Microsoft.AspNetCore.Mvc;
using SportStoreMvcNet6.Models;

namespace SportStoreMvcNet6.Components
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private Cart cart;

        public CartSummaryViewComponent(Cart cartService) {
            cart = cartService;
        }

        public IViewComponentResult Invoke() {
            return View(cart);
        }
    }
}
