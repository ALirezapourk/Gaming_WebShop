namespace MerchStore.Application.Common.Interfaces;

/// <summary>
/// Interface for the Unit of Work pattern.
/// This defines operations for transactional work that spans multiple repositories.
/// </summary>
/// <remarks>
/// The Unit of Work pattern offers several significant benefits in a Clean Architecture and CQRS setup:
///
/// 1. Atomicity: Ensures that a series of database operations either all succeed or all fail,
///    maintaining data integrity across multiple repository operations.
///
/// 2. Consistency: Maintains the consistent state of the database by grouping changes
///    and applying them as a single unit.
///
/// 3. Performance Optimization: Reduces database roundtrips by batching multiple
///    changes into a single transaction.
///
/// 4. Domain Invariants Protection: Ensures that domain rules spanning multiple entities
///    are enforced as a whole, preventing partial updates that could violate business rules.
///
/// 5. Separation from Repository Pattern: While repositories focus on data access operations
///    for specific entity types, UnitOfWork coordinates across all repositories.
///
/// 6. CQRS Support: In a CQRS architecture, commands often need to update multiple aggregates
///    atomically, which UnitOfWork facilitates.
///
/// 7. Transaction Management: Provides explicit control over transaction boundaries,
///    offering more granular control than implicit transactions.
///
/// 8. Testability: Makes it easier to test business logic by allowing transaction
///    rollback after tests, leaving the database in its original state.
/// </remarks>
public interface IUnitOfWork
{
    /// <summary>
    /// Saves all changes made in the context to the database
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation if needed</param>
    /// <returns>The number of affected entities</returns>
    /// <remarks>
    /// This method persists all tracked changes to the database but operates outside
    /// of an explicit transaction. For simple operations affecting a single entity or
    /// when transaction coordination isn't necessary, this offers better performance.
    /// </remarks>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins a new transaction
    /// </summary>
    /// <remarks>
    /// This starts a new database transaction that will encompass all subsequent
    /// data operations until explicitly committed or rolled back. This creates
    /// a transactional boundary to ensure multiple operations succeed or fail as a unit.
    ///
    /// In CQRS, this is typically called at the beginning of a command handler that
    /// will modify multiple aggregates or entities.
    /// </remarks>
    Task BeginTransactionAsync();

    /// <summary>
    /// Commits all changes made in the current transaction
    /// </summary>
    /// <remarks>
    /// This finalizes all changes made within the current transaction and persists them
    /// to the database. Once committed, the changes cannot be rolled back, and the
    /// transaction is complete.
    ///
    /// In a Command pattern, this is typically called at the end of a command handler
    /// after all business operations have been successfully performed.
    /// </remarks>
    Task CommitTransactionAsync();

    /// <summary>
    /// Rolls back all changes made in the current transaction
    /// </summary>
    /// <remarks>
    /// This discards all changes made within the current transaction, reverting the
    /// database to its state before the transaction began. This is typically called
    /// when an error occurs during processing or when business rules are violated.
    ///
    /// In error handling scenarios, this ensures no partial updates are applied,
    /// maintaining data consistency even when operations fail.
    /// </remarks>
    Task RollbackTransactionAsync();
}