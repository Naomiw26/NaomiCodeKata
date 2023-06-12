namespace RPGCombatKata.Infrastructure
{
    public interface ICharacterWriter
    {
        Task<bool> CreateCharacter(CharacterRecord record);
        Task<bool> UpdateCharacter(CharacterRecord record);
    }
}
