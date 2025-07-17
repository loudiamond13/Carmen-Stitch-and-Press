
using Carmen_Stitch_and_Press.Utilities;
using CSP.DAL.DbContexts;
using CSP.DAL.Interceptors;
using CSP.DAL.Repositories;
using CSP.Domain.IRepositories;
using CSP.Domain.Logics;
using CSP.Domain.Logics.Interfaces;
using CSP.Domain.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("CarmenStitchAndPressContextConnection") ?? throw new InvalidOperationException("Connection string 'CarmenStitchAndPressContextConnection' not found.");

builder.Services.AddDbContext<CarmenStitchAndPressDBContext>((provider, options) =>
{
    options.UseSqlServer(connectionString);
    options.AddInterceptors(provider.GetRequiredService<OrderAuditAndTotalInterceptor>());
});



builder.Services.AddIdentity<CarmenStitchAndPressUserModel, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<CarmenStitchAndPressDBContext>().AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews().AddCookieTempDataProvider(); 

builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();
builder.Services.AddScoped<OrderAuditAndTotalInterceptor>();

builder.Services.AddScoped<IIdentityUserServices, IdentityUserServicesLogic>();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddScoped<IExpenseRepository, ExpensesRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IMoneyTransferRepository, MoneyTransferRepository>();

//logics
builder.Services.AddScoped<IOrderItemLogic, OrderItemLogic>();
builder.Services.AddScoped<IOrderLogic, OrderLogic>();
builder.Services.AddScoped<IPaymentLogic, PaymentLogic>();
builder.Services.AddScoped<IDiscountLogic, DiscountLogic>();
builder.Services.AddScoped<IExpenseLogic, ExpenseLogic>();
builder.Services.AddScoped<IMoneyTransferLogic, MoneyTransferLogic>();
//builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);


builder.Services.AddRazorPages();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
 //   options.ExpireTimeSpan = TimeSpan.FromDays(365);
    options.SlidingExpiration = false;


    options.AccessDeniedPath = "/";
    options.LoginPath = "/";
});

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "App_Data", "Keys")))
    .SetApplicationName("CarmenStitchAndPress");

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
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
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);



app.MapRazorPages();
app.MapControllers();
app.Run();
