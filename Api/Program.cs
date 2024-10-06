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

var app = builder.Build();

//--------------------------------------------------
// Configure the HTTP request pipeline.

app.MapControllers();

//--------------------------------------------------
// Run the app.

app.Run();
