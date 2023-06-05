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
            var characters = await db.Characters.ToListAsync();
            if (characters != null)
            {
                return new GetAllCharactersResponse
                {
                    characters = characters,
                    Status = HttpStatusCode.OK,
                    Message = "Success."
                };
            }
            return new GetAllCharactersResponse
            {
                characters = null,
                Status = HttpStatusCode.NotFound,
                Message = "Success."
            };
        }

        public async Task<GetCharacterResponse> GetCharacter(Guid characterId, CharactersDb db)
        {
            var character = await db.Characters.FindAsync(characterId);
            if (character != null)
            {
                return new GetCharacterResponse
                {
                    character = character,
                    Status = HttpStatusCode.OK,
                    Message = "Success."
                };
            }
            return new GetCharacterResponse
            {
                character = null,
                Status = HttpStatusCode.NotFound,
                Message = "Success."
            };


        }

        public async Task<CreateCharacterResponse> CreateCharacter(CharactersDb db)
        {
            var character = new Character { id = new Guid(), health = Character.MaxHealth, isAlive = true, level = 1 };
            db.Add(character);
            await db.SaveChangesAsync();
            return new CreateCharacterResponse
            {
                character = character,
                Status = HttpStatusCode.OK,
                Message = "Success."
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
            var character = await db.Characters.FindAsync(characterId);
            if (character != null)
            {
                character.health -= amount;
                db.Characters.Update(character);
                await db.SaveChangesAsync();
                return new ApplyDamageResponse
                {
                    damageDealt = amount,
                    Status = HttpStatusCode.OK,
                    Message = "Success."
                };
            }
            return new ApplyDamageResponse
            {
                damageDealt = 0,
                Status = HttpStatusCode.NotFound,
                Message = "Success."
            };
        }

        private async Task<ApplyDamageResponse> HealCharacter(Guid characterId, int amount, CharactersDb db)
        {
            var character = await db.Characters.FindAsync(characterId);
            if (character != null)
            {
                character.health += amount;
                db.Characters.Update(character);
                await db.SaveChangesAsync();
                return new ApplyDamageResponse
                {
                    damageDealt = amount,
                    Status = HttpStatusCode.OK,
                    Message = "Success."
                };
            }
            return new ApplyDamageResponse
            {
                damageDealt = 0,
                Status = HttpStatusCode.NotFound,
                Message = "Success."
            };
}
    }
}
