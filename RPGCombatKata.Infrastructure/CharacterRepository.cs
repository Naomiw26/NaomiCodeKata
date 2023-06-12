﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPGCombatKata.Domain;
using RPGCombatKata.Domain.Data;

namespace RPGCombatKata.Infrastructure
{
    public class CharacterRepository : ICharacterWriter, ICharacterReader
    {
        private CharactersDb _charactersDb;
        private Mapper mapper;

        public CharacterRepository(CharactersDb charactersDb)
        {
            _charactersDb = charactersDb;
            mapper = MapperConfig.InitializeAutomapper();
        }

        public async Task<bool> AddCharacter(Character character)
        {
            var characterRecord = mapper.Map<CharacterRecord>(character);
            _charactersDb.Characters.Add(characterRecord);
            await _charactersDb.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCharacter(Character character)
        {
            var characterRecord = mapper.Map<CharacterRecord>(character);
            _charactersDb.Characters.Update(characterRecord);
            await _charactersDb.SaveChangesAsync();
            return true;
        }

        public async Task<List<Character>> GetAllCharacters()
        {
            var characterRecords = await _charactersDb.Characters.ToListAsync();
            var characters = new List<Character>();
            foreach (var record in characterRecords)
            {
                characters.Add(mapper.Map<Character>(record));
            }     
            return characters;
        }

        public async Task<Character> GetCharacter(Guid characterId)
        {
            var characterRecord = await _charactersDb.Characters.AsNoTracking().FirstOrDefaultAsync(c => c.id == characterId);
            var character = mapper.Map<Character>(characterRecord);
            return character;
        }

    }
}