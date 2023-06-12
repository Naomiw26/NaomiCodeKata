using RPGCombatKata.Api.Characters;
using RPGCombatKata.Api.Data;
using RPGCombatKata.Domain;
using RPGCombatKata.Domain.Data;
using System.Net;

namespace RPGCombatKata.Api.Service.Services
{
    public class CharactersService : ICharactersService
    {
        private ICharacterReader _characterReader;
        private ICharacterWriter _characterWriter;

        public CharactersService(ICharacterWriter characterWriter,ICharacterReader characterReader)
        {
            _characterReader = characterReader;
            _characterWriter = characterWriter;
        }

        public async Task<GetAllCharactersResponse> GetAllCharacters()
        {
            var characters = await _characterReader.GetAllCharacters();
            if (characters != null)
            {
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
            var character = await _characterReader.GetCharacter(characterId);
            if (character != null)
            {
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
            var character = new Character();
            character.id = Guid.NewGuid();
            await _characterWriter.AddCharacter(character);
  
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
            var character = await _characterReader.GetCharacter(characterId);
            if (character != null)
            {
                character.health -= amount;
                var damageDealt = amount;
                if (character.health < 0)
                {
                    damageDealt += character.health;
                    character.health = 0;
                }

                await _characterWriter.UpdateCharacter(character);

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
            var character = await _characterReader.GetCharacter(characterId);
            if (character != null)
            {               
                if (character.isAlive)
                {
                    character.health += amount;
                    var damageDealt = amount;
                    if (character.health > Character.MaxHealth)
                    {
                        damageDealt -= (character.health - Character.MaxHealth);
                        character.health = Character.MaxHealth;           
                    }

                    await _characterWriter.UpdateCharacter(character);

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
