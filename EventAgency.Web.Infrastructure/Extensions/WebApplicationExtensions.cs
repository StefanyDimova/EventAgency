using EventAgency.Data.Seeding.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventAgency.Web.Infrastructure.Extensions
{
    public static class WebApplicationExtensions
    {
        public static IApplicationBuilder SeedDefaultIdentity(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            IServiceProvider serviceProvider = scope.ServiceProvider;

            IIdentitySeeder identitySeeder = serviceProvider.GetRequiredService<IIdentitySeeder>();
            identitySeeder.SeedIdentityAsync().GetAwaiter().GetResult();

            return app;
        }
    }
}
