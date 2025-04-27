using MerchStore.Application.Common.Interfaces;
using MerchStore.Domain.Interfaces;

namespace MerchStore.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implementation of the Repository Manager pattern.
/// This class provides a single point of access to all repositories and the unit of work.
/// </summary>
public class RepositoryManager : IRepositoryManager
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Constructor that accepts all required repositories and the unit of work
    /// </summary>
    /// <param name="productRepository">The product repository</param>
    /// <param name="orderRepository">The order repository</param>
    /// <param name="unitOfWork">The unit of work</param>
    public RepositoryManager(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public IProductRepository ProductRepository => _productRepository;


    /// <inheritdoc/>
    public IUnitOfWork UnitOfWork => _unitOfWork;
}