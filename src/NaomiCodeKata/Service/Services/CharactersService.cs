using RPGCombatKata.Api.Characters;
using RPGCombatKata.Data;
using System.Net;
using Microsoft.EntityFrameworkCore;


namespace RPGCombatKata.Service.Services
{
    public class CharactersService : ICharactersService
    {
        public async Task<GetAllCharactersResponse> GetAllCharacters(CharactersDb db)
        {
            var characterRecords = await db.Characters.ToListAsync();
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

        public async Task<GetCharacterResponse> GetCharacter(Guid characterId, CharactersDb db)
        {
            var characterRecord = await db.Characters.FindAsync(characterId);
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

        public async Task<CreateCharacterResponse> CreateCharacter(CharactersDb db)
        {
            var characterRecord = new CharacterRecord { id = new Guid(), health = Character.MaxHealth,};
            db.Add(characterRecord);
            await db.SaveChangesAsync();
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

        public async Task<ApplyDamageResponse> ApplyDamage(Guid characterId, ApplyDamageRequest request, CharactersDb db)
        {
            switch (request.damageType)
            {
                case DamageType.Normal :
                   return await DamageCharacter(characterId,request.amount, db);
                case DamageType.Healing:
                   return await HealCharacter(characterId, request.amount, db);
                default:
                    return new ApplyDamageResponse { 
                        damageDealt = 0, 
                        Status = HttpStatusCode.NotFound , 
                        Message = "DamageType was not valid."
                    };
            }        
        }

        private async Task<ApplyDamageResponse> DamageCharacter(Guid characterId, int amount, CharactersDb db)
        {
            var characterRecord = await db.Characters.FindAsync(characterId);
            if (characterRecord != null)
            {
                characterRecord.health -= amount;
                var damageDealt = amount;
                if (characterRecord.health < 0)
                {
                    damageDealt += characterRecord.health;
                    characterRecord.health = 0;
                }

                db.Characters.Update(characterRecord);
                await db.SaveChangesAsync();

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

        private async Task<ApplyDamageResponse> HealCharacter(Guid characterId, int amount, CharactersDb db)
        {
            var characterRecord = await db.Characters.FindAsync(characterId);
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

                    db.Characters.Update(characterRecord);
                    await db.SaveChangesAsync();

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
