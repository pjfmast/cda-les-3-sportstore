using Microsoft.EntityFrameworkCore;
using SportStoreMvcNet6.Models;
using System.Web.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<StoreDbContext>(opts => {
    opts.UseSqlServer(
        builder.Configuration["ConnectionStrings:SportsStoreConnection"]);
});

builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();
builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();
builder.Services.AddMvc();

//  storing session state in memory, which is the approach here.
// This has the advantage of simplicity, but it means that the session data is lost when the application is stopped or restarted.  
//builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession();
builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();



app.UseRequestLocalization(opts => {
    opts.AddSupportedCultures("en-US")
    .AddSupportedUICultures("en-US")
    .SetDefaultCulture("en-US");
});

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthorization();


app.MapControllerRoute(
   name: null,
   pattern: "{category}/Page{productPage}",
   defaults: new { Controller = "Product", action = "List" });


app.MapControllerRoute(
    name: null,
    pattern: "Page{productPage:int}",
    defaults: new { Controller = "Product", action = "List", productPage = 1 });

app.MapControllerRoute(
    name: null,
    pattern: "{category}",
    defaults: new { Controller = "Product", action = "List", productPage = 1 });

app.MapControllerRoute(
    name: null,
    pattern: "",
    defaults: new { Controller = "Product", action = "List", productPage = 1 });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=List}/{id?}");

SeedData.EnsurePopulated(app);

app.Run();
