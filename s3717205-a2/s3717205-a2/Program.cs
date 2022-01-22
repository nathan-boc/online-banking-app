using Microsoft.EntityFrameworkCore;
using MvcBank.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<MvcBankContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("MvcBankContext")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed data
using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // fix here
        SeedData.Initialise(services);
    }
    catch(Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occured seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
