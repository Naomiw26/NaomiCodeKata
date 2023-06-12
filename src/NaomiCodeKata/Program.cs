using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RPGCombatKata.Api.Characters;
using RPGCombatKata.Api.Service.Services;
using RPGCombatKata.Domain;
using RPGCombatKata.Infrastructure;


var appBuilder = WebApplication.CreateBuilder(args);


var services = appBuilder.Services;
services.AddDbContext<CharactersDb>(opt => opt.UseInMemoryDatabase("Characters"));
services.AddScoped<CharacterRepository>();
services.AddScoped<ICharacterReader>(x => x.GetRequiredService<CharacterRepository>());
services.AddScoped<ICharacterWriter>(x => x.GetRequiredService<CharacterRepository>());
services.AddScoped<CharactersService>();

using var app = appBuilder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.MapGet("/characters", async (CharactersService service) =>
    Results.Ok(await service.GetAllCharacters())
);

app.MapGet("/characters/{id}", async ([FromRoute] Guid id, CharactersService service) =>
    Results.Ok(await service.GetCharacter(id))
);

app.MapPost("/characters", async (CharactersService service) =>
     Results.Ok(await service.CreateCharacter())
);

app.MapPost("/characters/{id}/_applyDamage", async([FromRoute] Guid id, [FromBody] ApplyDamageRequest request, CharactersService service) =>
     Results.Ok(await service.ApplyDamage(id, request))
);

app.Run();

public partial class Program
{

}

