using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MerchStore.WebUI.Models.Api.Basic;
using MerchStore.Application.Services.Interfaces;

namespace MerchStore.WebUI.Controllers.Api.Products;

/// <summary>
/// Basic API controller for read-only product operations.
/// Requires API Key authentication.
/// </summary>
[Route("api/basic/products")]
[ApiController]
[Authorize(Policy = "ApiKeyPolicy")] // ✅ Added API key auth
public class BasicProductsApiController : ControllerBase
{
    private readonly ICatalogService _catalogService;

    /// <summary>
    /// Constructor with dependency injection
    /// </summary>
    /// <param name="catalogService">The catalog service for accessing product data</param>
    public BasicProductsApiController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    /// <summary>
    /// Gets all products
    /// </summary>
    /// <returns>A list of all products</returns>
    /// <response code="200">Returns the list of products</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BasicProductDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var products = await _catalogService.GetAllProductsAsync();

            var productDtos = products.Select(p => new BasicProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price.Amount,
                Currency = p.Price.Currency,
                ImageUrl = p.ImageUrl?.ToString(),
                StockQuantity = p.StockQuantity
            });

            return Ok(productDtos);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An error occurred while retrieving products" });
        }
    }

    /// <summary>
    /// Gets a specific product by ID
    /// </summary>
    /// <param name="id">The ID of the product to retrieve</param>
    /// <returns>The requested product</returns>
    /// <response code="200">Returns the requested product</response>
    /// <response code="404">If the product is not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BasicProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var product = await _catalogService.GetProductByIdAsync(id);

            if (product is null)
            {
                return NotFound(new { message = $"Product with ID {id} not found" });
            }

            var productDto = new BasicProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price.Amount,
                Currency = product.Price.Currency,
                ImageUrl = product.ImageUrl?.ToString(),
                StockQuantity = product.StockQuantity
            };

            return Ok(productDto);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An error occurred while retrieving the product" });
        }
    }
}
