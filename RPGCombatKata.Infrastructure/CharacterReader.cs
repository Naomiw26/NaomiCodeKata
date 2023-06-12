using RPGCombatKata.Infrasturcture;

namespace RPGCombatKata.Infrastructure
{

    public class CharacterReader : ICharacterReader
    {
        public async Task<List<CharacterRecord>> GetAllCharacters()
        {
           // var characterRecords = await db.Characters.ToListAsync(
            var characterRecords = new List<CharacterRecord>();
            return characterRecords;
        }

        public async Task<CharacterRecord> GetCharacter(Guid characterId)
        {
           // var characterRecord = await db.Characters.FindAsync(characterId);
           var characterRecord = new CharacterRecord();
            return characterRecord;
        }
    }
}
