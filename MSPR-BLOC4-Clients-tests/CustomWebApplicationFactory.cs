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
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ClientDBContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<ClientDBContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });
        });
    }
}