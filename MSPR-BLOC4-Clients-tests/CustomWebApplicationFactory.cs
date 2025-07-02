using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MSPR_bloc_4_customers.Data;
using System.Linq;

namespace MSPR_BLOC4_Clients_tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Supprime l'ancien DbContext SQL Server
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ClientDBContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Ajoute le DbContext InMemory pour les tests
                services.AddDbContext<ClientDBContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                // Crée la base de données et l'initialise si nécessaire
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<ClientDBContext>();
                    db.Database.EnsureCreated();
                }
            });
        }
    }
}
