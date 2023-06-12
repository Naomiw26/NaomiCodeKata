namespace RPGCombatKata.Infrastructure
{
    public interface ICharacterReader
    {
        Task<List<CharacterRecord>> GetAllCharacters();
        Task<CharacterRecord> GetCharacter(Guid characterId);
    }
}
