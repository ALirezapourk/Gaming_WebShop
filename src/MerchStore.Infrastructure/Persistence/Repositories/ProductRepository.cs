using MerchStore.Domain.Entities;
using MerchStore.Domain.Interfaces;

namespace MerchStore.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository implementation for managing Product entities.
/// This class inherits from the generic Repository class and adds product-specific functionality.
/// </summary>
public class ProductRepository : Repository<Product, Guid>, IProductRepository
{
    /// <summary>
    /// Constructor that passes the context to the base Repository class
    /// </summary>
    /// <param name="context">The database context</param>
    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    // You can add product-specific methods here if needed
}