using RPGCombatKata.Domain.Data;

namespace RPGCombatKata.Domain
{
    public interface ICharacterWriter
    {
        Task<bool> AddCharacter(Character character);
        Task<bool> UpdateCharacter(Character character);
    }
}
