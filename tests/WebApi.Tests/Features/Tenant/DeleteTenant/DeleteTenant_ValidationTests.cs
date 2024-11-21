using Microsoft.EntityFrameworkCore;

namespace WalmgateIdentity.WebApi.Tests.Features.Tenant.DeleteTenant;
public class DeleteTenant_ValidationTests(ApiApplicationFixture fixture) : UsingApi(fixture)
{
    [Fact( DisplayName = "DELETE /tenant/{tenantId} when tenant does not exist should return 404")]
    public async Task Delete_WhenTenantDoesNotExist_ShouldReturn404()
    {
        // Given
        var tenantId = Guid.NewGuid();

        // When
        var response = await Fixture.Api.DeleteAsync($"tenants/{tenantId}");

        // Then
        response.StatusCode.Should().Be( HttpStatusCode.NotFound );
    }
}
