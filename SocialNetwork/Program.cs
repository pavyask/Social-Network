using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.Data;
var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<SocialNetworkContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("SocialNetworkContext")
//    ?? throw new InvalidOperationException("Connection string 'SocialNetworkContext' not found.")));


// Add services to the container.
builder.Services.AddSingleton(typeof(SocialNetworkData));
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.Configure<CookiePolicyOptions>(
 options => {
     options.CheckConsentNeeded = context => false;
     options.MinimumSameSitePolicy = SameSiteMode.None;
 });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePagesWithRedirects("/Error/NotFound");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithRedirects("/Error/NotFound");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{login?}");

app.Run();
