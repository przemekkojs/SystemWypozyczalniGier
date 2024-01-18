using System;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using SystemWypozyczalniGier.Database;
using SystemWypozyczalniGier.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SystemWypozyczalniGier
{
    //add migration & update database (mac)
    //dotnet ef migrations add nameOfMigration --project SystemWypozyczalniGier.csproj
    //dotnet ef database update --project SystemWypozyczalniGier.csproj

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContextPool<DatabaseContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("RentalShopDb")));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {

            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            options.LoginPath = "/Account/Login"; // Ścieżka do strony logowania
            options.AccessDeniedPath = "/Account/AccessDenied"; // Ścieżka dla dostępu zabronionego
            options.LogoutPath = "/Account/Logout";
            options.SlidingExpiration = true;
        });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
            });




            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<UserManager<ApplicationUser>>();

            var serviceProvider = services.BuildServiceProvider();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            IdentityHelper.EnsureRoleExists(roleManager, "Admin");
            IdentityHelper.EnsureRoleExists(roleManager, "Klient");

            IdentityHelper.AddAdmin(serviceProvider);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    public static class IdentityHelper
    {
        public static async void EnsureRoleExists(RoleManager<IdentityRole> roleManager, string roleName)
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        public static void AddAdmin(ServiceProvider serviceProvider)
        {
            const string adminEmail = "admin@example.com";
            const string adminPassword = "P@ssw0rd";

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (userManager.FindByEmailAsync(adminEmail).Result == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                };

                var result = userManager.CreateAsync(adminUser, adminPassword).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(adminUser, "Admin").Wait();
                }
            }
        }
    }
}

