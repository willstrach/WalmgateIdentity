
using Microsoft.Extensions.DependencyInjection;
using WalmgateIdentity.Core.Interfaces;
using WalmgateIdentity.WebApi.Tests.Infrastructure;

namespace WalmgateIdentity.WebApi.Tests.Fixtures;

public class UsingApi(ApiApplicationFixture fixture) : IClassFixture<ApiApplicationFixture>, IAsyncLifetime
{
    public ApiApplicationFixture Fixture { get; } = fixture;

    public async Task DisposeAsync() => await Fixture.ResetDatabaseAsync();

    public Task InitializeAsync() => Task.CompletedTask;
}

[CollectionDefinition("Using test API")]
public class ApiApplicationCollection : ICollectionFixture<ApiApplicationFixture>
{
    // Intentionally left blank
}

public class ApiApplicationFixture : IAsyncLifetime
{
    private ContainerisedDatabase _containerisedDatabase = null!;
    private ApiApplicationFactory _apiApplicationFactory = null!;

    public TestTime Time => _apiApplicationFactory.Time;
    public TestCurrentUser CurrentUser => _apiApplicationFactory.CurrentUser;
    public HttpClient Api => _apiApplicationFactory.CreateClient();

    public IDatabaseContext CreateDatabaseContext() => _apiApplicationFactory.CreateDatabaseContext();
    public IServiceScope CreateScope() => _apiApplicationFactory.CreateScope();

    public async Task InitializeAsync()
    {
        _containerisedDatabase = new ContainerisedDatabase();
        await _containerisedDatabase.InitializeAsync();
        _apiApplicationFactory = new ApiApplicationFactory(_containerisedDatabase.ConnectionString);
    }

    public async Task DisposeAsync()
    {
        await _containerisedDatabase.ResetDatabaseAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        await _containerisedDatabase.ResetDatabaseAsync();
    }
}
