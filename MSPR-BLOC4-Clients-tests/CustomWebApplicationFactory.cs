using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MSPR_bloc_4_customers.Data;
using System.Linq;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // Trouve et retire la configuration pr�c�dente du DbContext avec SQL Server
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ClientDBContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Ajoute le DbContext avec InMemory pour les tests
            services.AddDbContext<ClientDBContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            // Cr�e la base et ajoute �ventuellement des donn�es de test si besoin
            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ClientDBContext>();

                db.Database.EnsureCreated();

                // Optionnel : seed des donn�es pour tests
                // db.Clients.Add(new Client { Nom = "TestClient", ... });
                // db.SaveChanges();
            }
        });
    }
}
