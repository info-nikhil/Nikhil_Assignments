using ResortBookingUI;
using ResortBookingUI.MVC.Services;

namespace ResortBookingUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ---------------- MVC ----------------
            builder.Services.AddControllersWithViews();

            // ---------------- SESSION ----------------
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // ---------------- HTTP CONTEXT ----------------
            builder.Services.AddHttpContextAccessor();

            // READ FROM appsettings.json
            var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"];

            // ---------------- HTTP CLIENT ----------------
            builder.Services.AddHttpClient<ApiService>(client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            });

            var app = builder.Build();

            // ---------------- PIPELINE ----------------
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // SESSION (correct placement)
            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Login}/{id?}"
            );

            app.Run();
        }
    }
}