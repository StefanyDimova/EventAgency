using EventAgency.Services.Core.Interfaces;
using EventAgency.Web.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using static EventAgency.Web.ViewModels.ValidationMessages.Event;

namespace EventAgency.Web.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            this.productService = productService;
            this.categoryService = categoryService;
        }

        [HttpGet]

        public async Task<IActionResult> Index()
        {
            IEnumerable<AllProductsViewModel> allProducts = await this.productService
                .GetAllProductsAsync();

            //if (this.isUserAuthenticated())
            //{
            //    foreach (AllProductsViewModel productIndexVM in allProducts)
            //    {
            //        productIndexVM.IsAddedToUserWatchlist = await this.watchlistService
            //            .IsMovieAddedToWatchlist(movieIndexVM.Id, this.GetUserId());
            //    }
            //}


            return View(allProducts);
        }

        [HttpGet]
        public async Task<IActionResult> ByCategory(int id)
        {
            try
            {
                IEnumerable<AllProductsViewModel> products = await this.productService.GetProductsByCategoryIdAsync(id);

                return this.View("Index", products); // използваме същия изглед като Index
            }
            catch (Exception e)
            {
                // Може да добавиш логика за логване
                Console.WriteLine(e.Message);
                return this.RedirectToAction("Index");
            }
        }


        [HttpGet]
        public async Task<IActionResult> Add()
        {
            try
            {
                AddProductInputModel model = new AddProductInputModel()
                {
                    Categories = await this.categoryService.GetCategoriesDropdownDataAsync()
                };

                return this.View(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProductInputModel inputModel)
        {
            // modelState пази стейта на моделната валидация за този момент
            if (!this.ModelState.IsValid)
            {
                // пренасочва ни към вюто за създаване 
                // данните които са били попълнени ще си останат , зашото сме подали inputModel на вюто
                return this.View(inputModel);
            }


            // пробвай да добавиш направения модел
            try
            {
                await this.productService.AddProductAsync(inputModel);

                // ako добавянето е успешно пренасочваме потребителя към главната страница с филми , което е Index
                return this.RedirectToAction(nameof(Index), "Category");
            }
            catch (Exception e)
            {
                // TODO: Implement it with the ILogger
                Console.WriteLine(e.Message);


                // тук си добавяме наша грешка , която си е наша
                this.ModelState.AddModelError(string.Empty, ServiceCreateError);
                return this.View(inputModel);
            }
        }

    }
}
