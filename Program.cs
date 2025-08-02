using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShaTaskApp.Models;
using ShaTaskApp.Models.Context;
using ShaTaskApp.Repositories;
using ShaTaskApp.Repositories.Interfaces;
using ShaTaskApp.Services;
using ShaTaskApp.Services.Interfaces;

namespace ShaTaskApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ShaTaskDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            })
             .AddEntityFrameworkStores<ShaTaskDbContext>()
             .AddDefaultTokenProviders();


            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IInvoiceService, InvoiceService>();
            builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();


            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
