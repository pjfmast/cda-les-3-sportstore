using Microsoft.AspNetCore.Mvc;
using SportStoreMvcNet6.Models;
using SportStoreMvcNet6.Models.ViewModels;

namespace SportStoreMvcNet6.Controllers
{
    public class CartController : Controller
    {
        private readonly IStoreRepository repository;
        private readonly Cart cart;

        public CartController(IStoreRepository repo, Cart cartService) {
            repository = repo;
            cart = cartService;
        }

        public ViewResult Index(string? returnUrl) {
            return View(new CartIndexViewModel {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToActionResult AddToCart(int productId, string returnUrl) {
            Product? product = repository.Products.FirstOrDefault(p => p.ProductID == productId);

            if (product != null) {
                cart.AddItem(product, 1);
            }
            var result = RedirectToAction("Index", new { returnUrl });
            return result;
        }

        public RedirectToActionResult RemoveFromCart(long productId, string returnUrl) {
            //Product? product = repository.Products.FirstOrDefault(p => p.ProductID == productId);

            cart.RemoveLine(cart.Lines.First(cl =>
                cl.Product.ProductID == productId).Product);

            //  This has the effect of sending an HTTP redirect instruction to the client browser, asking the browser to request a new URL. 
            //    In this case, the browser is asked to request a URL that will call the Index action method of the CartController.
            // See the method Index(...) up in this class.
            return RedirectToAction("Index", new { returnUrl });
        }
    }
}
