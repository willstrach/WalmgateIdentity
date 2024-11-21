using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WalmgateIdentity.WebApi.Helpers;

namespace WalmgateIdentity.WebApi.Tests.Helpers;
public class PagedListExtensionsTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<TestDbContext> _contextOptions;

    public PagedListExtensionsTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        _contextOptions = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = new TestDbContext(_contextOptions);
        context.Database.EnsureCreated();
    }

    public void Dispose() => _connection.Dispose();

    private TestDbContext CreateDbContext() => new(_contextOptions);

    [Fact]
    public async Task ToPagedListAsync_OnFirstPage_ShouldReturnFirstNEntities()
    {
        // Arrange
        var context = CreateDbContext();

        var entities = Enumerable.Range(0, 100).Select(number => new Entity { Name = $"Entity number {number}" });
        context.Entities.AddRange(entities);
        await context.SaveChangesAsync();

        // Act
        var pagedList = await context.Entities
            .OrderBy(entity => entity.Id)
            .ToPagedListAsync(1, 10, CancellationToken.None);
        
        // Assert
        pagedList.Page.Should().Be(1);
        pagedList.PageSize.Should().Be(10);
        pagedList.TotalCount.Should().Be(100);
        pagedList.TotalPages.Should().Be(10);
        pagedList.Items.Should().HaveCount(10);

        pagedList.Items
            .Select(entity => entity.Name)
            .Should()
            .BeEquivalentTo(
                "Entity number 0",
                "Entity number 1",
                "Entity number 2",
                "Entity number 3",
                "Entity number 4",
                "Entity number 5",
                "Entity number 6",
                "Entity number 7",
                "Entity number 8",
                "Entity number 9"
            );
    }

    [Fact]
    public async Task ToPagedListAsync_OnLastPage_ShouldReturnLastNEntities()
    {
        // Arrange
        var context = CreateDbContext();

        var entities = Enumerable.Range(0, 100).Select(number => new Entity { Name = $"Entity number {number}" });
        context.Entities.AddRange(entities);
        await context.SaveChangesAsync();

        // Act
        var pagedList = await context.Entities
            .OrderBy(entity => entity.Id)
            .ToPagedListAsync(10, 10, CancellationToken.None);

        // Assert
        pagedList.Page.Should().Be(10);
        pagedList.PageSize.Should().Be(10);
        pagedList.TotalCount.Should().Be(100);
        pagedList.TotalPages.Should().Be(10);
        pagedList.Items.Should().HaveCount(10);

        pagedList.Items
            .Select(entity => entity.Name)
            .Should()
            .BeEquivalentTo(
                "Entity number 90",
                "Entity number 91",
                "Entity number 92",
                "Entity number 93",
                "Entity number 94",
                "Entity number 95",
                "Entity number 96",
                "Entity number 97",
                "Entity number 98",
                "Entity number 99"
            );
    }

    class Entity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    class TestDbContext : DbContext
    {
        public DbSet<Entity> Entities { get; set; } = null!;

        public TestDbContext(DbContextOptions<TestDbContext> contextOptions) : base(contextOptions) { }
    }
}
