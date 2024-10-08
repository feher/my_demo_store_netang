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

var app = builder.Build();

//--------------------------------------------------
// Configure the HTTP request pipeline.

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
