using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//--------------------------------------------------
// Add services to the container (i.e. configure dependency injection).

builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddCors();

var app = builder.Build();

//--------------------------------------------------
// Configure the HTTP request pipeline.

// Allow requests from our web client.
// The web client lives at localhost:4200 (different domain than the this server API).
app.UseCors(b => b.AllowAnyHeader()
                  .AllowAnyMethod()
                  .WithOrigins("http://localhost:4200", "https://localhost:4200"));
app.MapControllers();

//--------------------------------------------------
// Misc.

try
{
    using var services = app.Services.CreateScope();
    var context = services.ServiceProvider.GetService<StoreContext>()!;

    // Apply pending migrations.
    await context.Database.MigrateAsync();

    // Seed some test data to the database.
    await StoreContextSeed.SeedDataAsync(context);
}
catch (System.Exception ex)
{
    Console.WriteLine(ex);
    throw;
}

//--------------------------------------------------
// Run the app.

app.Run();
