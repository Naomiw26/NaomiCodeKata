using RPGCombatKata.Domain.Data;

namespace RPGCombatKata.Domain
{
    public interface ICharacterReader
    {
        Task<List<Character>> GetAllCharacters();
        Task<Character> GetCharacter(Guid characterId);
    }
}
