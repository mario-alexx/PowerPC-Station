using API.Middleware;
using API.SignalR;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Add StoreContext to the service container with SQL Server configuration
builder.Services.AddDbContext<StoreContext>(opt => {
   // Configures the DbContext to use SQL Server with the connection string from the configuration
  opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Register the IProductRepository with its concrete implementation
builder.Services.AddScoped<IProductRepository, ProductRepository>();
// Register the IGenericRepository with its concrete implementation for any T entity
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
// Registers the UnitOfWork implementation in the dependency injection container
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add support for Cross-Origin Resource Sharing (CORS)
builder.Services.AddCors();
// Register a singleton instance of IConnectionMultiplexer with the Redis connection
builder.Services.AddSingleton<IConnectionMultiplexer>(config => 
{
  var connString = builder.Configuration.GetConnectionString("Redis") ??
    throw new Exception("Cannot get redis connection string");
  var configuration = ConfigurationOptions.Parse(connString, true);
  return ConnectionMultiplexer.Connect(configuration);
});

// Register the CartService as a singleton implementation of ICartService
builder.Services.AddSingleton<ICartService, CartService>();
// Adds authorization services to the dependency injection container.
builder.Services.AddAuthorization();
// Configures ASP.NET Core Identity to use the AppUser class for user management and adds the API endpoints for Identity.
builder.Services.AddIdentityApiEndpoints<AppUser>()
  .AddEntityFrameworkStores<StoreContext>();
// Register the PaymentService as a singleton implementation of IPaymentService
builder.Services.AddScoped<IPaymentService, PaymentService>();
// Registers SignalR 
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.

// Use the custom middleware for handling exceptions
app.UseMiddleware<ExceptionMiddleware>();

// Configure the CORS policy to allow specific headers, methods and credentials from defined origins
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials()
  .WithOrigins("http://localhost:4200","https://localhost:4200"));

// Configures middleware for authentication, authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
// Maps Identity API endpoints under the route "api" using the AppUser class.
app.MapGroup("api").MapIdentityApi<AppUser>();
// Configures mapping the notification hub
app.MapHub<NotificationHub>("/hub/notifications");

try
{
  // Create a scope for resolving scoped services
  using IServiceScope scope = app.Services.CreateScope();
  IServiceProvider services = scope.ServiceProvider;
  
  // Get the StoreContext from the services
  var context = services.GetRequiredService<StoreContext>();
  
  // Apply pending migrations to the database
  await context.Database.MigrateAsync();
 
  // Seed the database with initial data
  await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
  Console.WriteLine(ex);
  throw;
}

app.Run();
