using System;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using SystemWypozyczalniGier.Database;

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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

