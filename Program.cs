using AzureWebApp.Data;
using Microsoft.EntityFrameworkCore;
using YourProject.Services;
using Azure.Messaging.ServiceBus;

var builder = WebApplication.CreateBuilder(args);


// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Azure Blob Service
builder.Services.AddSingleton<BlobStorageService>();    

// Add Azure Servive Bus 
builder.Services.AddSingleton<ServiceBusSenderService>();
builder.Services.AddHostedService<ServiceBusReceiverBackgroundService>();

// Add services to the container.
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddControllersWithViews();

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
