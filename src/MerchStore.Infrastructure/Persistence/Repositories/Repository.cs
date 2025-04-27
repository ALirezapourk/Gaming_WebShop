using Microsoft.EntityFrameworkCore;
using MerchStore.Domain.Common;
using MerchStore.Domain.Interfaces;

namespace MerchStore.Infrastructure.Persistence.Repositories;

/// <summary>
/// A generic repository implementation that works with any entity type.
/// This provides standard CRUD operations for all entities.
/// </summary>
/// <typeparam name="TEntity">The entity type this repository works with</typeparam>
/// <typeparam name="TId">The ID type of the entity</typeparam>
public class Repository<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : notnull
{
    // The DbContext instance - protected so derived classes can access it
    protected readonly AppDbContext _context;

    // DbSet for the specific entity type
    protected readonly DbSet<TEntity> _dbSet;

    /// <summary>
    /// Constructor that accepts a DbContext
    /// </summary>
    /// <param name="context">The database context to use</param>
    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>(); // Get the DbSet for this entity type
    }

    /// <summary>
    /// Retrieves an entity by its ID
    /// </summary>
    /// <param name="id">The entity's ID</param>
    /// <returns>The entity if found, null otherwise</returns>
    public virtual async Task<TEntity?> GetByIdAsync(TId id)
    {
        return await _dbSet.FindAsync(id);
    }

    /// <summary>
    /// Retrieves all entities of this type
    /// </summary>
    /// <returns>A collection of all entities</returns>
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    /// <summary>
    /// Adds a new entity to the database
    /// </summary>
    /// <param name="entity">The entity to add</param>
    public virtual async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    /// <summary>
    /// Updates an existing entity in the database
    /// </summary>
    /// <param name="entity">The entity to update</param>
    public virtual Task UpdateAsync(TEntity entity)
    {
        // Mark the entity as modified
        _context.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Removes an entity from the database
    /// </summary>
    /// <param name="entity">The entity to remove</param>
    public virtual Task RemoveAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }
}