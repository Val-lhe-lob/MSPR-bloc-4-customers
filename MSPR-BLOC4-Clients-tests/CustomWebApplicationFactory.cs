using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authentication;
using MSPR_bloc_4_customers.Data;
using MSPR_bloc_4_customers.Models;
using System.Linq;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // Remove existing DbContext registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ClientDBContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            // Add InMemory DbContext
            services.AddDbContext<ClientDBContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            // Register Fake Authentication
            services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, FakeJwtAuthHandler>("Test", options => { });

            services.Configure<AuthenticationOptions>(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
            });

            // Build the service provider
            var sp = services.BuildServiceProvider();

            // Seed test data
            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ClientDBContext>();
                db.Database.EnsureCreated();

                db.Clients.Add(new Client
                {
                    IdClient = 1,
                    Nom = "Test Nom",
                    Prenom = "Test Prenom",
                    Adresse = "123 Rue de Test",
                    CodePostal = "75000",
                    Ville = "Paris"
                });
                db.SaveChanges();
            }
        });
    }
}
