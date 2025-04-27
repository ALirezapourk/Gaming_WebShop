using Microsoft.AspNetCore.Mvc;
using MerchStore.Application.Services.Interfaces;
using MerchStore.WebUI.Models.Catalog;

namespace MerchStore.WebUI.Controllers;

public class CatalogController : Controller
{
    private readonly ICatalogService _catalogService;

    public CatalogController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    // GET: Catalog
    public async Task<IActionResult> Index()
    {
        try
        {
            // Get all products from the service
            var products = await _catalogService.GetAllProductsAsync();

            // Map domain entities to view models
            var productViewModels = products.Select(p => new ProductCardViewModel
            {
                Id = p.Id,
                Name = p.Name,
                TruncatedDescription = p.Description.Length > 100
                    ? p.Description.Substring(0, 97) + "..."
                    : p.Description,
                FormattedPrice = p.Price.ToString(),
                PriceAmount = p.Price.Amount,
                ImageUrl = p.ImageUrl?.ToString(),
                StockQuantity = p.StockQuantity
            }).ToList();

            // Create the product catalog view model
            var viewModel = new ProductCatalogViewModel
            {
                FeaturedProducts = productViewModels
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            // Log the exception
            // In a real application, you should use a proper logging framework
            Console.WriteLine($"Error in ProductCatalog: {ex.Message}");

            // Show an error message to the user
            ViewBag.ErrorMessage = "An error occurred while loading products. Please try again later.";
            return View("Error");
        }
    }

    // GET: Store/Details/5
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            // Get the specific product from the service
            var product = await _catalogService.GetProductByIdAsync(id);

            // Return 404 if product not found
            if (product is null)
            {
                return NotFound();
            }

            // Map domain entity to view model
            var viewModel = new ProductDetailsViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                FormattedPrice = product.Price.ToString(),
                PriceAmount = product.Price.Amount,
                ImageUrl = product.ImageUrl?.ToString(),
                StockQuantity = product.StockQuantity
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in ProductDetails: {ex.Message}");

            // Show an error message to the user
            ViewBag.ErrorMessage = "An error occurred while loading the product. Please try again later.";
            return View("Error");
        }
    }
}