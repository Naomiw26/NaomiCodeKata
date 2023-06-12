namespace RPGCombatKata.Infrastructure
{
    public class CharacterWriter : ICharacterWriter
    {
        public async Task<bool> CreateCharacter(CharacterRecord record)
        {
            //db.Add(characterRecord);
            //await db.SaveChangesAsync();
            throw new NotImplementedException();
        }
        
        public async Task<bool> UpdateCharacter(CharacterRecord record)
        {
            //db.Characters.Update(characterRecord);
            //await db.SaveChangesAsync();
            throw new NotImplementedException();
        }
    }
}
