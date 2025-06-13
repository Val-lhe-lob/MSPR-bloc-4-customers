using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using MSPR_bloc_4_customers.Controllers;
using MSPR_bloc_4_customers.Models;
using MSPR_bloc_4_customers.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

public class ClientsControllerTests
{
    private ClientDBContext GetDbContextWithData()
    {
        var options = new DbContextOptionsBuilder<ClientDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // nom unique à chaque test
            .Options;

        var context = new ClientDBContext(options);

        context.Clients.AddRange(
            new Client { IdClient = 1, Nom = "Dupont", Prenom = "Jean", Ville = "Paris", CodePostal = "75000", Entreprise = "test" },
            new Client { IdClient = 2, Nom = "Martin", Prenom = "Marie", Ville = "Lyon", CodePostal = "69000", Entreprise = "test2" }
        );
        context.SaveChanges();

        return context;
    }

    [Fact]
    public async Task GetAll_ReturnsAllClients()
    {
        // Arrange
        var context = GetDbContextWithData();
        var controller = new ClientsController(context);

        // Act
        var result = await controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var clients = Assert.IsAssignableFrom<IEnumerable<Client>>(okResult.Value);
        Assert.Equal(2, clients.Count());
    }

    [Fact]
    public async Task GetById_ReturnsClient_WhenExists()
    {
        // Arrange
        var context = GetDbContextWithData();
        var controller = new ClientsController(context);

        // Act
        var result = await controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var client = Assert.IsType<Client>(okResult.Value);
        Assert.Equal("Dupont", client.Nom);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenNotExists()
    {
        // Arrange
        var context = GetDbContextWithData();
        var controller = new ClientsController(context);

        // Act
        var result = await controller.GetById(999);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
}