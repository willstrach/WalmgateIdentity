using System.Reflection;
using FluentValidation;
using WalmgateIdentity.Core.Interfaces;
using WalmgateIdentity.Infrastructure;
using WalmgateIdentity.WebApi.Middleware.Auth;
using WalmgateIdentity.WebApi.Middleware.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ICurrentUser, CurrentUserService>();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddMediatR(configuration => {
    configuration.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
    configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
});
var app = builder.Build();

app.MapEndpoints();

app.Run();


public partial class Program {} // Makes program public so integration tests can use it
