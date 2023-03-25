using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceMVC.Models;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration
    .GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationContext>
    (o => o.UseSqlServer(connString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(o => o.LoginPath = "/login");

builder.Services.AddScoped<AccountService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(o => o.MapControllers());

app.UseStaticFiles();

app.Run();