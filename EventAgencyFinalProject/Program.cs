using EventAgencyFinalProject.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EventAgency.Web.Infrastructure.Extensions;
using EventAgency.Data.Repository.Interfaces;
using EventAgency.Services.Core.Interfaces;

namespace EventAgencyFinalProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.
                AddDbContext<EventAgencyDbContext>(options =>
                {
                    options.UseSqlServer(connectionString);

                });
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                ConfigureIdentity(builder.Configuration, options);
            })
                .AddEntityFrameworkStores<EventAgencyDbContext>();



            builder.Services.AddRepositories(typeof(IEventRepository).Assembly);
            builder.Services.AddUserDefinedServices(typeof(IEventService).Assembly);

            builder.Services.AddRazorPages();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithRedirects("Home/Error?statusCode={0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }

        private static void ConfigureIdentity(IConfigurationManager configurationManager, IdentityOptions identityOptions)
        {
            identityOptions.SignIn.RequireConfirmedEmail =
                configurationManager.GetValue<bool>($"IdentityConfiguration:SignIn:RequireConfirmedEmail");
            identityOptions.SignIn.RequireConfirmedAccount =
                configurationManager.GetValue<bool>($"IdentityConfiguration:SignIn:RequireConfirmedAccount");
            identityOptions.SignIn.RequireConfirmedPhoneNumber =
                configurationManager.GetValue<bool>($"IdentityConfiguration:SignIn:RequireConfirmedPhoneNumber");

            identityOptions.Password.RequiredLength =
                configurationManager.GetValue<int>($"IdentityConfiguration:Password:RequiredLength");
            identityOptions.Password.RequireNonAlphanumeric =
                configurationManager.GetValue<bool>($"IdentityConfiguration:Password:RequireNonAlphanumeric");
            identityOptions.Password.RequireDigit =
                configurationManager.GetValue<bool>($"IdentityConfiguration:Password:RequireDigit");
            identityOptions.Password.RequireLowercase =
                configurationManager.GetValue<bool>($"IdentityConfiguration:Password:RequireLowercase");
            identityOptions.Password.RequireUppercase =
                configurationManager.GetValue<bool>($"IdentityConfiguration:Password:RequireUppercase");
            identityOptions.Password.RequiredUniqueChars =
                configurationManager.GetValue<int>($"IdentityConfiguration:Password:RequiredUniqueChars");
        }
    }
}
