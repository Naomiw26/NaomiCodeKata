using Microsoft.AspNetCore.Mvc;
using RPGCombatKata.Api.Characters;
using RPGCombatKata.Api.Service.Services;

namespace RPGCombatKata.Api.Service.Controllers
{
    public class CharactersController
    {
        public CharactersController(WebApplication app)
        {

            app.MapGet("/characters", async (CharactersService service) =>
                Results.Ok(await service.GetAllCharacters())
            );

            app.MapGet("/characters/{id}", async ([FromRoute] Guid id, CharactersService service) =>
                Results.Ok(await service.GetCharacter(id))
            );

            app.MapPost("/characters", async (CharactersService service) =>
                 Results.Ok(await service.CreateCharacter())
            );

            app.MapPost("/characters/{id}/_applyDamage", async ([FromRoute] Guid id, [FromBody] ApplyDamageRequest request, CharactersService service) =>
                 Results.Ok(await service.ApplyDamage(id, request))
            );
        }
    }
}
