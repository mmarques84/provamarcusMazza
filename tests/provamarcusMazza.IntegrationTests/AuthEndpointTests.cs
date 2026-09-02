using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using provamarcusMazza.Application.Auth.Commands.Login;
using Xunit;

namespace provamarcusMazza.IntegrationTests;

public sealed class AuthEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_WithExpectedCredentials_ShouldReturnToken()
    {
        var response = await _client.PostAsJsonAsync(
            "/auth/login",
            new LoginCommand("dev@martech.com", "Senha@123"));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.False(string.IsNullOrWhiteSpace(body?.AccessToken));
    }
}
