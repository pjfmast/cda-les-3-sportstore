using Microsoft.AspNetCore.Mvc;
using SportStoreMvcNet6.Models;
using SportStoreMvcNet6.Models.ViewModels;
using System.Diagnostics;

namespace SportStoreMvcNet6.Controllers
{
    public class ProductController : Controller
    {
        private IStoreRepository repository;
        public int PageSize = 4;

        public ProductController(IStoreRepository repo) {
            repository = repo;
        }

        public ViewResult List(string? category, int productPage = 1)
           => View(new ProductsListViewModel {
               Products = repository.Products
                   .Where(p => category == null || p.Category == category)
                   .OrderBy(p => p.ProductID)
                   .Skip((productPage - 1) * PageSize)
                   .Take(PageSize),
               PagingInfo = new PagingInfo {
                   CurrentPage = productPage,
                   ItemsPerPage = PageSize,
                   TotalItems = category == null
                        ? repository.Products.Count()
                        : repository.Products.Where(e =>
                            e.Category == category).Count()
               },
               CurrentCategory = category
           });
    }
}