using Inventory.Controllers;
using Inventory.Data;
using Inventory.IServices;
using Inventory.Services;
using Microsoft.EntityFrameworkCore;

//We meed builder variable for new service creation.
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
//Following servie is used for database migration.
builder.Services.AddDbContext<ApplicationContext>(option => {
    option.UseSqlServer(builder.Configuration.GetConnectionString("ProductionDB"));
});
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
