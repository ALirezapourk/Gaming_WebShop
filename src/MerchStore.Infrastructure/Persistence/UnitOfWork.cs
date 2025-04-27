using MerchStore.Application.Common.Interfaces;
using MerchStore.Infrastructure.Persistence;

namespace MerchStore.Infrastructure.Persistence;

/// <summary>
/// The Unit of Work pattern provides a way to group multiple database operations
/// into a single transaction that either all succeed or all fail together.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Constructor that accepts a DbContext
    /// </summary>
    /// <param name="context">The database context to use</param>
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Saves all changes made in the context to the database
    /// </summary>
    /// <returns>The number of affected entities</returns>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Begins a new transaction
    /// </summary>
    public async Task BeginTransactionAsync()
    {
        await _context.Database.BeginTransactionAsync();
    }

    /// <summary>
    /// Commits all changes made in the current transaction
    /// </summary>
    public async Task CommitTransactionAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }

    /// <summary>
    /// Rolls back all changes made in the current transaction
    /// </summary>
    public async Task RollbackTransactionAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }
}