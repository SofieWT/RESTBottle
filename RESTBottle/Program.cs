using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RESTBottle.Interfaces;
using RESTBottle.repostory;
using RESTBottle.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var optionsBuilder = new DbContextOptionsBuilder<BottlesDBContext>();
optionsBuilder.UseSqlServer(Secret.GetConnectionString);
BottlesDBContext _dbContext = new(optionsBuilder.Options);

builder.Services.AddSingleton<IBottlesRepository>(new BottleRepositoryDatabase(_dbContext));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
