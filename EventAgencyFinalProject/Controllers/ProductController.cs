using EventAgency.Services.Core.Interfaces;
using EventAgency.Web.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
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
            //        productIndexVM.IsAddedToCart = await this.watchlistService
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

        [HttpGet]
        [AllowAnonymous]

        public async Task<IActionResult> Details(string? id)
        {
            try
            {
                ProductDetailsViewModel? productDetails = await this.productService
                    .GetProductDetailsByIdAsync(id);

                if (productDetails == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View("Details", productDetails);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            try
            {
                ProductEditInputModel? editableProduct = await this.productService
                    .GetEditableProductByIdAsync(id);
                if (editableProduct == null)
                {
                    // TODO: Custom 404 page
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(editableProduct);
            }
            catch (Exception e)
            {
                // TODO: Implement it with the ILogger
                // TODO: Add JS bars to indicate such errors
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit(ProductEditInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                bool editSuccess = await this.productService.EditProductAsync(inputModel);
                if (!editSuccess)
                {
                    // TODO: Custom 404 page
                    return this.RedirectToAction(nameof(Index));
                }

                return this.RedirectToAction(nameof(Details), new { id = inputModel.Id });

            }
            catch (Exception e)
            {
                // TODO: Implement it with the ILogger
                // TODO: Add JS bars to indicate such errors
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }


        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            try
            {
                DeleteProductViewModel? productToBeDeleted = await this.productService
                    .GetProductDeleteDetailsByIdAsync(id);
                if (productToBeDeleted == null)
                {
                    // TODO: Custom 404 page
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(productToBeDeleted);
            }
            catch (Exception e)
            {
                // TODO: Implement it with the ILogger
                // TODO: Add JS bars to indicate such errors
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }


        [HttpPost]
        public async Task<IActionResult> Delete(DeleteProductViewModel inputModel)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    // TODO: Implement JS notifications
                    Console.WriteLine(">>> ModelState invalid!");
                    return this.RedirectToAction(nameof(Index));
                }

                bool deleteResult = await this.productService
                    .SoftDeleteProductAsync(inputModel.Id);
                if (deleteResult == false)
                {
                    // TODO: Implement JS notifications
                    // TODO: Alt_Redirect to Not Found page
                    return this.RedirectToAction(nameof(Index));
                }

                // TODO: Success notification
                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                // TODO: Implement it with the ILogger
                // TODO: Add JS bars to indicate such errors
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }

    }
}
