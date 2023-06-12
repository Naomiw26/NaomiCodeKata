
using RPGCombatKata.Api.Characters;

namespace RPGCombatKata.Service.Services
{
    public interface ICharactersService
    {
        Task<ApplyDamageResponse> ApplyDamage(Guid characterId, ApplyDamageRequest request, CharactersDb db);
        Task<CreateCharacterResponse> CreateCharacter(CharactersDb db);
        Task<GetAllCharactersResponse> GetAllCharacters(CharactersDb db);
        Task<GetCharacterResponse> GetCharacter(Guid characterId, CharactersDb db);
    }
}
