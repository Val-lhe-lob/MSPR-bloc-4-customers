using Microsoft.AspNetCore.Authentication;
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
            // Replace DbContext with InMemory
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ClientDBContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<ClientDBContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            //  Add Fake Authentication for integration tests
            services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, FakeJwtAuthHandler>(
                    "Test", options => { });
        });
    }
}

