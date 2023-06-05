using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using RPGCombatKata.Api.Characters;
using RPGCombatKata.Data;
using System.Net.Http;
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
    private async Task<Guid> AddCharacterToDb(HttpClient client)
    {
        var r = await client.PostAsync("characters", null);
        var sr = await r.Content.ReadAsStringAsync();
        var rsp = JsonConvert.DeserializeObject<GetCharacterResponse>(sr);
        return rsp.character.id;
    }

    [Fact]
    public async Task CreateCharacter()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsync("characters",null);
   
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<CreateCharacterResponse>(stringResponse);
        
        // Assert
        Assert.Equal(1,resp.character.level);
        Assert.Equal(1000, resp.character.health);
        Assert.True(resp.character.isAlive);
        Assert.Equal("Character Created Succesfully.", resp.Message);
    }

    [Fact]
    public async Task GetAllCharacters()
    {
        // Arrange
        var client = _factory.CreateClient();
        var characterId = await AddCharacterToDb(client);
        // Act
        var response = await client.GetAsync("characters");

        // Assert
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<GetAllCharactersResponse>(stringResponse);
        Assert.True(resp.characters.Count > 0);
        Assert.True(resp.characters.Exists(c => c.id.Equals(characterId))) ;
        Assert.Equal("Success, Returning Characters.", resp.Message);
    }

    [Fact]
    public async Task GetCharacter()
    {
        // Arrange
        var client = _factory.CreateClient();

        var characterId = await AddCharacterToDb(client);
        // Act
        var response = await client.GetAsync($"characters/{characterId}");

        // Assert
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<GetCharacterResponse>(stringResponse);
        Assert.True(resp.character.isAlive);
        Assert.Equal("Success. Returning Character.", resp.Message);
    }

    [Fact]
    public async Task GetCharacterInvalidId()
    {
        // Arrange
        var client = _factory.CreateClient();
        // Act
        var response = await client.GetAsync($"characters/{new Guid()}");

        // Assert
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<GetCharacterResponse>(stringResponse);
        Assert.Equal("CharacterID Not Found.", resp.Message);
    }

    [Fact]
    public async Task ApplyWrongTypeDamage()
    {
        // Arrange
        var client = _factory.CreateClient();

        var characterId = await AddCharacterToDb(client);

        var request = new ApplyDamageRequest
        {
            amount = 10,
            damageType = (DamageType)100
        };

        // Act
        var response = await client.PostAsJsonAsync($"characters/{characterId}/_applyDamage", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ApplyDamageResponse>(stringResponse);
        Assert.Equal(0, resp.damageDealt);
        Assert.Equal("DamageType was not valid.", resp.Message);
    }

    [Fact]
    public async Task ApplyNormalDamage()
    {
        // Arrange
        var client = _factory.CreateClient();

        var characterId = await AddCharacterToDb(client);

        var request = new ApplyDamageRequest
        {
            amount = 10,
            damageType = DamageType.Normal
        };

        // Act
        var response = await client.PostAsJsonAsync($"characters/{characterId}/_applyDamage", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ApplyDamageResponse>(stringResponse);
        Assert.Equal(10, resp.damageDealt);
        Assert.Equal("Character Damaged Succesfully.", resp.Message);
    }

    [Fact]
    public async Task ApplyNormalDamageInvalidId()
    {
        // Arrange
        var client = _factory.CreateClient();
        
        var request = new ApplyDamageRequest
        {
            amount = 10,
            damageType = DamageType.Normal
        };

        // Act
        var response = await client.PostAsJsonAsync($"characters/{new Guid()}/_applyDamage", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ApplyDamageResponse>(stringResponse);
        Assert.Equal(0, resp.damageDealt);
        Assert.Equal("CharacterID Not Found.", resp.Message);
    }

    [Fact]
    public async Task ApplyNormalDamageExcessDamage()
    {
        // Arrange
        var client = _factory.CreateClient();

        var characterId = await AddCharacterToDb(client);

        var request = new ApplyDamageRequest
        {
            amount = Character.MaxHealth + 100,
            damageType = DamageType.Normal
        };

        // Act
        var response = await client.PostAsJsonAsync($"characters/{characterId}/_applyDamage", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ApplyDamageResponse>(stringResponse);
        Assert.Equal(Character.MaxHealth, resp.damageDealt);
        Assert.Equal("Character Damaged Succesfully.", resp.Message);

    }

    [Fact]
    public async Task ApplyHealingDamage()
    {
        // Arrange
        var client = _factory.CreateClient();

        var characterId = await AddCharacterToDb(client);

        var request = new ApplyDamageRequest
        {
            amount = 100,
            damageType = DamageType.Normal
        };

        await client.PostAsJsonAsync($"characters/{characterId}/_applyDamage", request);
        request.damageType = DamageType.Healing;
        // Act
        var response = await client.PostAsJsonAsync($"characters/{characterId}/_applyDamage", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ApplyDamageResponse>(stringResponse);
        Assert.Equal(100, resp.damageDealt);
        Assert.Equal("Character Healed Succesfully.", resp.Message);
    }

    [Fact]
    public async Task ApplyHealingDamageExcessHealing()
    {
        // Arrange
        var client = _factory.CreateClient();

        var characterId = await AddCharacterToDb(client);

        var request = new ApplyDamageRequest
        {
            amount = 100,
            damageType = DamageType.Healing
        };

        // Act
        var response = await client.PostAsJsonAsync($"characters/{characterId}/_applyDamage", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ApplyDamageResponse>(stringResponse);
        Assert.True(resp.damageDealt == 0);
        Assert.Equal("Character Healed Succesfully.", resp.Message);
    }

    [Fact]
    public async Task ApplyHealingDamageTargetDead()
    {
        // Arrange
        var client = _factory.CreateClient();

        var characterId = await AddCharacterToDb(client);

        var request = new ApplyDamageRequest
        {
            amount = Character.MaxHealth,
            damageType = DamageType.Normal
        };

        await client.PostAsJsonAsync($"characters/{characterId}/_applyDamage", request);
        request.damageType = DamageType.Healing;

        // Act
        var response = await client.PostAsJsonAsync($"characters/{characterId}/_applyDamage", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ApplyDamageResponse>(stringResponse);
        Assert.True(resp.damageDealt == 0);
        Assert.Equal("Character is Dead and Can't be Healed.", resp.Message);
    }

    [Fact]
    public async Task ApplyHealingDamageInvalidId()
    {
        // Arrange
        var client = _factory.CreateClient();

        var request = new ApplyDamageRequest
        {
            amount = 10,
            damageType = DamageType.Healing
        };

        // Act
        var response = await client.PostAsJsonAsync($"characters/{new Guid()}/_applyDamage", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ApplyDamageResponse>(stringResponse);
        Assert.Equal(0, resp.damageDealt);
        Assert.Equal("CharacterID Not Found.", resp.Message);
    }


}

