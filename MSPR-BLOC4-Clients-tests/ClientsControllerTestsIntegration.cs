using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MSPR_BLOC4_Clients_tests
{
    public class ClientsControllerTestsIntegration : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        private string GenerateJwtToken()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("votre_cle_secrete_ici_au_moins_32_caracteres")); // 32+ caractères
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "MSPR4_CLIENT",
                audience: "MSPR4_CLIENT",
                claims: new[] { new Claim(ClaimTypes.Name, "testuser") },
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClientsControllerTestsIntegration()
        {
            var factory = new CustomWebApplicationFactory();
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllClients_ReturnsSuccess()
        {
            _client.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", GenerateJwtToken());

            // Arrange & Act
            var response = await _client.GetAsync("/api/clients");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}