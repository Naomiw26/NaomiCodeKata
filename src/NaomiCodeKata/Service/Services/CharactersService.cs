using RPGCombatKata.Api.Characters;
using RPGCombatKata.Api.Data;
using RPGCombatKata.Domain.Data;
using RPGCombatKata.Infrastructure;
using System.Net;

namespace RPGCombatKata.Api.Service.Services
{
    public class CharactersService : ICharactersService
    {
        public ICharacterReader _characterReader;
        public ICharacterWriter _characterWriter;
        public CharactersService(ICharacterReader characterReader, ICharacterWriter characterWriter )
        {
            _characterReader = characterReader;
            _characterWriter = characterWriter;
        }

        public async Task<GetAllCharactersResponse> GetAllCharacters()
        {
            var characterRecords = await _characterReader.GetAllCharacters();
            if (characterRecords != null)
            {
                var characters = new List<Character>();
                foreach (var characterRecord in characterRecords) {
                    characters.Add(new Character { 
                        health = characterRecord.health, 
                        id = characterRecord.id, 
                        level = characterRecord.level
                    });
                }
                return new GetAllCharactersResponse
                {
                    characters = characters,
                    Status = HttpStatusCode.OK,
                    Message = "Success, Returning Characters."
                };
            }
            return new GetAllCharactersResponse
            {
                characters = null,
                Status = HttpStatusCode.NotFound,
                Message = "No Characters Found."
            };
        }

        public async Task<GetCharacterResponse> GetCharacter(Guid characterId)
        {
            var characterRecord = await _characterReader.GetCharacter(characterId);
            if (characterRecord != null)
            {
                var character = (new Character
                {
                    health = characterRecord.health,
                    id = characterRecord.id,
                    level = characterRecord.level
                });
                return new GetCharacterResponse
                {
                    character = character,
                    Status = HttpStatusCode.OK,
                    Message = "Success. Returning Character."
                };
            }
            return new GetCharacterResponse
            {
                character = null,
                Status = HttpStatusCode.NotFound,
                Message = "CharacterID Not Found."
            };
        }

        public async Task<CreateCharacterResponse> CreateCharacter()
        {
            var characterRecord = new CharacterRecord { id = new Guid(), health = Character.MaxHealth,};
            await _characterWriter.CreateCharacter(characterRecord);
            var character = new Character
            {
                health = characterRecord.health,
                id = characterRecord.id,
                level = characterRecord.level
            };
            return new CreateCharacterResponse
            {
                character = character,
                Status = HttpStatusCode.OK,
                Message = "Character Created Succesfully."
            };
        }

        public async Task<ApplyDamageResponse> ApplyDamage(Guid characterId, ApplyDamageRequest request)
        {
            switch (request.damageType)
            {
                case DamageType.Normal :
                   return await DamageCharacter(characterId,request.amount);
                case DamageType.Healing:
                   return await HealCharacter(characterId, request.amount);
                default:
                    return new ApplyDamageResponse { 
                        damageDealt = 0, 
                        Status = HttpStatusCode.NotFound , 
                        Message = "DamageType was not valid."
                    };
            }        
        }

        private async Task<ApplyDamageResponse> DamageCharacter(Guid characterId, int amount)
        {
            var characterRecord = await _characterReader.GetCharacter(characterId);
            if (characterRecord != null)
            {
                characterRecord.health -= amount;
                var damageDealt = amount;
                if (characterRecord.health < 0)
                {
                    damageDealt += characterRecord.health;
                    characterRecord.health = 0;
                }

                await _characterWriter.UpdateCharacter(characterRecord);

                return new ApplyDamageResponse
                {
                    damageDealt = damageDealt,
                    Status = HttpStatusCode.OK,
                    Message = "Character Damaged Succesfully."
                };
            }
            return new ApplyDamageResponse
            {
                damageDealt = 0,
                Status = HttpStatusCode.NotFound,
                Message = "CharacterID Not Found."
            };
        }

        private async Task<ApplyDamageResponse> HealCharacter(Guid characterId, int amount)
        {
            var characterRecord = await _characterReader.GetCharacter(characterId);
            if (characterRecord != null)
            {
                var character = new Character { id = characterRecord.id , health = characterRecord.health , level = characterRecord.level};
                
                if (character.isAlive)
                {
                    character.health += amount;
                    var damageDealt = amount;
                    if (character.health > Character.MaxHealth)
                    {
                        damageDealt -= (character.health - Character.MaxHealth);
                        character.health = Character.MaxHealth;           
                    }

                    characterRecord.health = character.health;

                    await _characterWriter.UpdateCharacter(characterRecord);

                    return new ApplyDamageResponse
                    {
                        damageDealt = damageDealt,
                        Status = HttpStatusCode.OK,
                        Message = "Character Healed Succesfully."
                    };
                }
                return new ApplyDamageResponse
                {
                    damageDealt = 0,
                    Status = HttpStatusCode.OK,
                    Message = "Character is Dead and Can't be Healed."
                };
            }
            return new ApplyDamageResponse
            {
                damageDealt = 0,
                Status = HttpStatusCode.NotFound,
                Message = "CharacterID Not Found."
            };
}
    }
}
