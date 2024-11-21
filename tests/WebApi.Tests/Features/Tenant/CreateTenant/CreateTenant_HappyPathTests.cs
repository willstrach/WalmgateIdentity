using WalmgateIdentity.WebApi.Features.Tenants;

namespace WalmgateIdentity.WebApi.Tests.Features.Tenant.CreateTenant;

[Collection("Using test API")]
public class CreateTenant_HappyPathTests(ApiApplicationFixture fixture)
{
    public ApiApplicationFixture Fixture { get; set; } = fixture;

    [Fact( DisplayName = "POST /tenants with valid name should create tenant" )]
    public async Task Post_WithValidName_ShouldReturn200()
    {
        // Given
        var request = new { Name = "New Tenant" };

        // When
        var response = await Fixture.Api.PostAsJsonAsync("/tenants", request);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<TenantVm>();
        responseContent.Should().NotBeNull();
        responseContent!.Name.Should().Be("New Tenant");
        var newTenantId = responseContent.Id;

        var context = Fixture.CreateDatabaseContext();
        var tenant = await context.Tenants.FindAsync(newTenantId);
        tenant.Should().NotBeNull();
        tenant!.Name.Should().Be("New Tenant");
    }

    [Fact( DisplayName = "POST /tenants when deleted tenant with same name exists should create tenant" )]
    public async Task Post_WithDeletedTenant_ShouldReturn200()
    {
        // Given
        var context = Fixture.CreateDatabaseContext();
        context.Tenants.Add(new() { Name = "Existing Tenant", IsDeleted = true });
        await context.SaveChangesAsync(CancellationToken.None);
        
        var request = new { Name = "Existing Tenant" };

        // When
        var response = await Fixture.Api.PostAsJsonAsync("/tenants", request);

        // Then
        var responseContent = await response.Content.ReadFromJsonAsync<TenantVm>();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Should().NotBeNull();
        responseContent!.Name.Should().Be("Existing Tenant");
        var newTenantId = responseContent.Id;

        var tenant = await context.Tenants.FindAsync(newTenantId);
        tenant.Should().NotBeNull();
        tenant!.Name.Should().Be("Existing Tenant");
    }
}
