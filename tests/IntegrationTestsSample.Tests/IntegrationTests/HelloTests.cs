using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace IntegrationTestsSample.Tests.IntegrationTests;

// <snippet1>
public class BasicTests 
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public BasicTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("Hello");

        // Assert
        response.EnsureSuccessStatusCode(); 
        Assert.Equal("text/plain; charset=utf-8", 
            response.Content.Headers.ContentType.ToString());

       Assert.Equal("Hello World!",
            await response.Content.ReadAsStringAsync());
    }
}
// </snippet1>
