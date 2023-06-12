
using RPGCombatKata.Api.Characters;

namespace RPGCombatKata.Api.Service.Services
{
    public interface ICharactersService
    {
        Task<ApplyDamageResponse> ApplyDamage(Guid characterId, ApplyDamageRequest request);
        Task<CreateCharacterResponse> CreateCharacter();
        Task<GetAllCharactersResponse> GetAllCharacters();
        Task<GetCharacterResponse> GetCharacter(Guid characterId);
    }
}
