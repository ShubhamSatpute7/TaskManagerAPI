using TaskManagerAPI.Data;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=tasks.db"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Tasks.Any())
    {
        db.Tasks.AddRange(
            new TaskItem
            {
                Title = "Buy groceries",
                Description = "Milk, Bread, Eggs",
                DueDate = new DateTime(2025, 6, 1, 17, 0, 0),
                IsComplete = false
            },
            new TaskItem
            {
                Title = "Finish report",
                Description = "Complete the financial report",
                DueDate = new DateTime(2025, 6, 5, 12, 0, 0),
                IsComplete = false
            },
            new TaskItem
            {
                Title = "Call Mom",
                Description = "Weekly catch-up call",
                DueDate = null,
                IsComplete = true
            }
        );
        db.SaveChanges();
    }
}

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