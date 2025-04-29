using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpLogging;
using KitchenDashboard.Server.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
        policy.WithOrigins("https://localhost:7079")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// HTTP request/response logging
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.All;
});

// EF Core with SQLite
builder.Services.AddDbContext<ChoresDbContext>(options =>
    options.UseSqlite("Data Source=chores.db"));

// Use the EF-backed repository
builder.Services.AddScoped<IChoreRepository, EfChoreRepository>();

var app = builder.Build();

// Enable HTTP logging middleware
app.UseHttpLogging();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}

// Serve Blazor assets and static files
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.UseCors("AllowBlazorClient");
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
