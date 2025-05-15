using System;
using Business.Context;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// DbContext ekleniyor
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Session ekleniyor
builder.Services.AddSession();

// MVC Controller + View ekleniyor
builder.Services.AddControllersWithViews();

// Authentication ekleniyor, standart cookie scheme ile
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login";  // Giriş yapılmadan erişim istenirse buraya yönlendiraaaaa
        options.AccessDeniedPath = "/Error/Forbidden"; // Yetkisiz erişim için opsiyonel
        options.ExpireTimeSpan = TimeSpan.FromHours(7);
        options.SlidingExpiration = true;
    });

// Authorization ekleniyor
builder.Services.AddAuthorization();

var app = builder.Build();

// Hata yönetimi ve güvenlik
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/Error500");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Öncelikle session aktif edilmeli, sonra authentication ve authorization
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Durum kodu sayfaları (örneğin 404)
app.UseStatusCodePagesWithReExecute("/Error/Status", "?code={0}");

// Route ayarı
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

app.Run();
