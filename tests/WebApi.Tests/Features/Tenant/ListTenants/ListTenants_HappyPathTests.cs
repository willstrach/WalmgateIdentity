using Microsoft.AspNetCore.WebUtilities;
using WalmgateIdentity.WebApi.Features.Tenants;
using WalmgateIdentity.WebApi.Models;

namespace WalmgateIdentity.WebApi.Tests.Features.Tenant.ListTenants;
public class ListTenants_HappyPathTests(ApiApplicationFixture fixture) : UsingApi(fixture)
{
    [Fact(DisplayName = "GET /tenants when no tenants exist should return 200")]
    public async Task Get_WhenPageIs1AndPageSizeIs10_ShouldReturn200()
    {
        // Given
        var request = new Dictionary<string, string?>
        {
            { "page", "1" },
            { "pageSize", "10" }
        };
        var requestUri = QueryHelpers.AddQueryString("tenants", request);

        // When
        var response = await Fixture.Api.GetAsync(requestUri);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseBody = await response.Content.ReadFromJsonAsync<PagedList<TenantVm>>();
        responseBody.Should().NotBeNull();
        responseBody?.Items.Should().BeEmpty();
    }

    [Fact(DisplayName = "GET /tenants when tenants exist should return 200")]
    public async Task Get_WhenTenantsExist_ShouldReturn200()
    {
        // Given
        var context = Fixture.CreateDatabaseContext();
        
        context.Tenants.Add(new() { Id = Guid.NewGuid(), Name = "Tenant 1" });
        context.Tenants.Add(new() { Id = Guid.NewGuid(), Name = "Tenant 2" });
        context.Tenants.Add(new() { Id = Guid.NewGuid(), Name = "Tenant 3" });

        await context.SaveChangesAsync(CancellationToken.None);

        var request = new Dictionary<string, string?>
        {
            { "page", "1" },
            { "pageSize", "10" }
        };

        var requestUri = QueryHelpers.AddQueryString("tenants", request);

        // When
        var response = await Fixture.Api.GetAsync(requestUri);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseBody = await response.Content.ReadFromJsonAsync<PagedList<TenantVm>>();
        responseBody.Should().NotBeNull();
        responseBody?.Items.Should().HaveCount(3);
    }

    [Fact(DisplayName = "GET /tenants when search has value should filter results and return 200")]
    public async Task Get_WhenSearchHasValue_ShouldFilterResultsAndReturn200()
    {
        // Given
        var context = Fixture.CreateDatabaseContext();

        context.Tenants.Add(new() { Id = Guid.NewGuid(), Name = "Tenant 1" });
        context.Tenants.Add(new() { Id = Guid.NewGuid(), Name = "Tenant 2" });
        context.Tenants.Add(new() { Id = Guid.NewGuid(), Name = "Tenant 3" });
        await context.SaveChangesAsync(CancellationToken.None);
        
        var request = new Dictionary<string, string?>
        {
            { "page", "1" },
            { "pageSize", "10" },
            { "search", "Tenant 2" }
        };
        var requestUri = QueryHelpers.AddQueryString("tenants", request);

        // When
        var response = await Fixture.Api.GetAsync(requestUri);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseBody = await response.Content.ReadFromJsonAsync<PagedList<TenantVm>>();
        responseBody.Should().NotBeNull();
        responseBody?.Items.Should().HaveCount(1);
        responseBody?.Items.First().Name.Should().Be("Tenant 2");
    }
}
