using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RPGCombatKata.Api.Characters;
using RPGCombatKata.Service.Services;

var appBuilder = WebApplication.CreateBuilder(args);

var services = appBuilder.Services;
services.AddDbContext<CharactersDb>(opt => opt.UseInMemoryDatabase("Characters"));
services.AddSingleton<CharactersService>();

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

app.MapGet("/characters", async (CharactersService service, CharactersDb db) =>
    Results.Ok(await service.GetAllCharacters(db))
);

app.MapGet("/characters/{id}", async ([FromRoute] Guid id, CharactersService service, CharactersDb db) =>
    Results.Ok(await service.GetCharacter(id,db))
);

app.MapPost("/characters", async (CharactersService service, CharactersDb db) =>
     Results.Ok(await service.CreateCharacter(db))
);

app.MapPost("/characters/{id}/_applyDamage", async([FromRoute] Guid id, [FromBody] ApplyDamageRequest request, CharactersService service, CharactersDb db) =>
     Results.Ok(await service.ApplyDamage(id, request, db))
);

app.Run();

public partial class Program
{

}

