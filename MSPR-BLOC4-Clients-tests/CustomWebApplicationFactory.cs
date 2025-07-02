using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using MSPR_bloc_4_customers.Data;
using System.Linq;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Supprimer le DbContext SQL Server
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ClientDBContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Ajouter le DbContext InMemory
            services.AddDbContext<ClientDBContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            // Configurer l'authentification fake
            services.AddAuthentication("Test")
                .AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions, FakeJwtAuthHandler>(
                    "Test", options => { });

            // Définir le schéma d'authentification par défaut
            services.Configure<Microsoft.AspNetCore.Authentication.AuthenticationOptions>(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
            });

            // Créer la base et l'initialiser si besoin
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ClientDBContext>();
                db.Database.EnsureCreated();

                // Optionnel : vider la table et ajouter des données initiales pour les tests
                if (!db.Clients.Any())
                {
                    db.Clients.Add(new MSPR_bloc_4_customers.Models.Client
                    {
                        IdClient = 1,
                        Nom = "Test Client",
                    });
                    db.SaveChanges();
                }
            }
        });
    }
}
