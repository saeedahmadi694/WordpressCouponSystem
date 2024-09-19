using Coupon.Context;
using Coupon.Context.Factory;
using Coupon.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region DI

builder.Services.AddSingleton<IDbContextSeed, DataContextSeed>();
var dbContextOptions = new DbContextOptionsBuilder<CouponDataContext>().UseSqlServer(builder.Configuration.GetConnectionString("CouponDatabase")).Options;
builder.Services.AddSingleton(dbContextOptions);
var assembly = AppDomain.CurrentDomain.Load("Coupon");

builder.Services.AddSingleton<UniBookDbContextOptions>();
builder.Services.AddSingleton<ICouponContextFactory, CouponDataContextFactory>();
builder.Services.AddDbContext<CouponDataContext>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

#endregion

#region Repositories

builder.Services.AddScoped<ICreditRepository, CreditRepository>();

#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
