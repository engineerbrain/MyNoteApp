using Microsoft.EntityFrameworkCore;
using mynotes.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DatabaseContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Session i�lemleri

// Session i�in gerekli olan Distributed Memory Cache servisini ekleyin
builder.Services.AddDistributedMemoryCache();

// Session ayarlar�n� ekleyin
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Oturum zaman a��m� s�resi
    options.Cookie.HttpOnly = true; // G�venlik i�in HTTP �zerinden eri�ilebilir
    options.Cookie.IsEssential = true; // Cookie esast�r ve consent gerektirmez
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

app.UseRouting();


// Session middleware'�n� ekleyin
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
