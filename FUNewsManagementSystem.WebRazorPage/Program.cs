using FUNewsManagementSystem.DataAccess;
using FUNewsManagementSystem.Services;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
//sigmalR
builder.Services.AddSignalR();
// Đăng ký Database và Configuration
builder.Services.AddDatabaseAndConfiguration(builder.Configuration);
builder.Services.AddAuthentication("Cookies")
    .AddCookie(options =>
    {
        options.LoginPath = "/SystemAccounts/LoginPage";
        options.LogoutPath = "/SystemAccounts/Logout";
    });

builder.Services.AddRazorPages();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddSession();
var app = builder.Build();
app.UseSession();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.MapHub<SignalrServer>("/SignalrServer");
app.MapRazorPages();

app.Run();
