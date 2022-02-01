using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

using MvcBank.Models.DataManager;
using MvcBank.Data;
using MvcBank.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<MvcBankContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MvcBankContext"));

    // Enable lazy loading
    options.UseLazyLoadingProxies();
});

builder.Services.AddScoped<CustomerManager>();
builder.Services.AddScoped<AccountManager>();
builder.Services.AddScoped<LoginManager>();
builder.Services.AddScoped<TransactionManager>();
builder.Services.AddScoped<BillPayManager>();
builder.Services.AddScoped<PayeeManager>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add BillPay service to run in the background
builder.Services.AddHostedService<BillPayService>();

// Store session data in web server memory
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    // Toggle session cookies on
    options.Cookie.IsEssential = true;
});

// Add services to the container
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


var app = builder.Build();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        SeedData.Initialise(services, builder.Configuration.GetConnectionString("RestApiUrl"));
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occured seeding the database.");
    }
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();