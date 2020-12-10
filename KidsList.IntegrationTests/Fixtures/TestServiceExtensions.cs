using System.Linq;
using Microsoft.EntityFrameworkCore;
using KidsList.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using KidsList.AuthService;

namespace KidsList.IntegrationTests
{
    public static class TestServiceExtensions
    {
        public static void AddTestDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            // Remove the main db context created by Api.Startup
            var originalDbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<KidsListContext>));

            services.Remove(originalDbContextDescriptor);

            // Now add a test db context using the test database.
            services.AddPostgres(configuration, "IntegrationTestsContext");

            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<KidsListContext>();

            db.Database.EnsureCreated();
        }

        public static void AddTestAuthentication(
            this IServiceCollection services, IAuthService testAuthService)
        {
            services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

            var auth0Descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IAuthService));

            services.Remove(auth0Descriptor);

            services.AddSingleton(sp => testAuthService);
        }
    }
}
