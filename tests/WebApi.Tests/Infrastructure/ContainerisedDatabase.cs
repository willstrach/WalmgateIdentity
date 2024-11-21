using Microsoft.EntityFrameworkCore;
using Npgsql;
using NSubstitute;
using Respawn;
using Testcontainers.PostgreSql;
using WalmgateIdentity.Core.Interfaces;
using WalmgateIdentity.Infrastructure.Database;

namespace WalmgateIdentity.WebApi.Tests.Infrastructure;

public class ContainerisedDatabase : IAsyncLifetime
{
    private readonly PostgreSqlContainer _databaseContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("walmgateidentity")
        .WithUsername("test-user")
        .WithPassword("test-password")
        .WithReuse(true)
        .WithPortBinding(5432, true)
        .Build();

    private readonly ICurrentUser _currentUser = Substitute.For<ICurrentUser>();
    private readonly ITime _time = Substitute.For<ITime>();
    private Respawner _respawner = null!;

    public string ConnectionString => _databaseContainer.GetConnectionString();

    public async Task InitializeAsync()
    {
        await _databaseContainer.StartAsync();

        var contextOptions = new DbContextOptionsBuilder<DatabaseContext>().UseNpgsql(ConnectionString, options =>
        {
            options.MigrationsHistoryTable("__EFMigrationsHistory", "EF");
        }).Options;
        var context = new DatabaseContext(contextOptions, _currentUser, _time);
        context.Database.Migrate();

        using var connection = new NpgsqlConnection(ConnectionString);
        await connection.OpenAsync();
        _respawner = await Respawner.CreateAsync(connection, new RespawnerOptions()
        {
            SchemasToInclude = ["admin"],
            DbAdapter = DbAdapter.Postgres
        });
    }

    public async Task DisposeAsync()
    {
        await ResetDatabaseAsync();
        await _databaseContainer.StopAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        using var connection = new NpgsqlConnection(ConnectionString);
        await connection.OpenAsync();
        await _respawner.ResetAsync(connection);
    }
}
