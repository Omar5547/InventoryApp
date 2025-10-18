using Application.Interfaces;
using Application.Services;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repository;
using InventoryApp.Provider;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<InventoryAppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IVendorService, VendorService>();
builder.Services.AddScoped<IPurchaseOrderService, PurchOrderService>();
builder.Services.AddScoped<ISalesOrderService, SalesOrderService>();
builder.Services.AddScoped<ProductProvider>();
builder.Services.AddScoped<PurchaseOrderProvider>();
builder.Services.AddScoped<VendorProvider>();
builder.Services.AddScoped<CustomerProvider>();
builder.Services.AddScoped<CategoryProvider>();
builder.Services.AddScoped<SalesOrderProvider>();
builder.Services.AddHttpContextAccessor();


builder.Services.AddIdentity<ApplicationUser,IdentityRole>(Options =>
{
    Options.Password.RequireDigit = true; // 1345
    Options.Password.RequireNonAlphanumeric = true; // @# $
    Options.Password.RequireUppercase = true; // A B C
    Options.Password.RequireLowercase = true; // a b c
})
    .AddEntityFrameworkStores<InventoryAppDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(Options => 
{ 
Options.LoginPath = "/Account/Login";
Options.AccessDeniedPath = "/Account/AccessDenied";
}
);

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");

app.Run();
