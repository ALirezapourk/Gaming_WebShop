using MerchStore.Domain.Common;

namespace MerchStore.Domain.UnitTests.Common;

// Helper class to test the abstract Entity<TId> base class
public class TestEntity : Entity<Guid>
{
    public TestEntity(Guid id) : base(id) { }

    protected TestEntity() { }
}

public class EntityTests
{
    [Fact]
    public void Constructor_WithValidId_SetsId()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var entity = new TestEntity(id);

        // Assert
        Assert.Equal(id, entity.Id);
    }

    [Fact]
    public void Constructor_WithDefaultId_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new TestEntity(Guid.Empty));
    }

    [Fact]
    public void Equals_WithSameId_ReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);

        // Act & Assert
        Assert.True(entity1.Equals(entity2));
    }

    [Fact]
    public void Equals_WithDifferentId_ReturnsFalse()
    {
        // Arrange
        var entity1 = new TestEntity(Guid.NewGuid());
        var entity2 = new TestEntity(Guid.NewGuid());

        // Act & Assert
        Assert.False(entity1.Equals(entity2));
    }

    [Fact]
    public void EqualsOperator_WithSameId_ReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);

        // Act & Assert
        Assert.True(entity1 == entity2);
    }

    [Fact]
    public void NotEqualsOperator_WithDifferentId_ReturnsTrue()
    {
        // Arrange
        var entity1 = new TestEntity(Guid.NewGuid());
        var entity2 = new TestEntity(Guid.NewGuid());

        // Act & Assert
        Assert.True(entity1 != entity2);
    }

    [Fact]
    public void GetHashCode_WithSameId_ReturnsSameHashCode()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);

        // Act & Assert
        Assert.Equal(entity1.GetHashCode(), entity2.GetHashCode());
    }
}