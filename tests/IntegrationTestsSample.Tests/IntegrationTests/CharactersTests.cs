using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using RPGCombatKata.Api.Characters;
using System.Security.Claims;
using Xunit;

namespace RPGCombatKata.Tests.IntegrationTests;

// <snippet1>
public class CharacterTests 
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CharacterTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateCharacter()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsync("characters",null);

        // Assert
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<CreateCharacterResponse>(stringResponse);
        Assert.Equal(1,resp.character.level);
        Assert.Equal(100, resp.character.health);
        Assert.True(resp.character.isAlive);
    }

    [Fact]
    public async Task GetAllCharacters()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("characters");

        // Assert
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<GetAllCharactersResponse>(stringResponse);
        Assert.True(resp.characters.Count > 0);
    }
}
// </snippet1>
