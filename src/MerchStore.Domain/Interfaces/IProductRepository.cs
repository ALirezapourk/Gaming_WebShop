using MerchStore.Domain.Entities;

namespace MerchStore.Domain.Interfaces;

public interface IProductRepository : IRepository<Product, Guid>
{
    // You can add product-specific methods here if needed
}