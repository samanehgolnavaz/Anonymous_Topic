using Anonymous_Topics.Database;
using Anonymous_Topics.Services.Implementations;
using Anonymous_Topics.Services.Intefaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IGuidGenerator,GuidGenerator>();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));

});
// setting up the Homepage as the landing page  
builder.Services.AddMvc().AddRazorPagesOptions(options =>
{
    options.Conventions.AddPageRoute("/TopicsGrid", "");
});

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


await ApplyDatabaseCreation(app);
async Task ApplyDatabaseCreation(WebApplication webApplication)
{
    await using var serviceScope = webApplication.Services.CreateAsyncScope();

    var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    var appliedMigration = (await dbContext.Database.GetAppliedMigrationsAsync()).ToList();

    var sqlScript = dbContext.Database.GenerateCreateScript();

    await dbContext.Database.MigrateAsync();
}

app.UseRouting();

app.UseAuthorization();
app.MapDefaultControllerRoute();
app.MapRazorPages();

//app.MapControllerRoute(
   // name: "default",
   // pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();
