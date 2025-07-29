using EventAgency.Data.Repository.Interfaces;
using EventAgency.Services.Core.Interfaces;
using EventAgencyFinalProject.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EventAgency.Web.Infrastructure.Extensions;

namespace EventAgency.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                                      ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddAuthorization();

            // Configuring Identity for API usage
            builder.Services.AddIdentityApiEndpoints<IdentityUser>()
                .AddEntityFrameworkStores<EventAgencyDbContext>();

            builder.Services.AddRepositories(typeof(IEventRepository).Assembly);
            builder.Services.AddUserDefinedServices(typeof(IEventService).Assembly);

            builder.Services.AddDbContext<EventAgencyDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            // Add CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Swagger only for development environment
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Redirect HTTP to HTTPS
            app.UseHttpsRedirection();


            // Enable Authorization middleware
            app.UseAuthorization();

            // Enable CORS
            app.UseCors("AllowAll");

            // Map controllers
            app.MapControllers();

            // Run the application
            app.Run();
        }
    }
}
