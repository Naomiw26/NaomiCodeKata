using RPGCombatKata.Api.Characters;
using RPGCombatKata.Api.Data;
using RPGCombatKata.Api.Service.Exceptions;
using RPGCombatKata.Api.Service.Factories;
using RPGCombatKata.Domain;
using RPGCombatKata.Domain.Data;
using System.Net;

namespace RPGCombatKata.Api.Service.Services
{
    public class CharactersService : ICharactersService
    {
        private ICharacterReader _characterReader;
        private ICharacterWriter _characterWriter;

        public CharactersService(ICharacterWriter characterWriter, ICharacterReader characterReader)
        {
            _characterReader = characterReader;
            _characterWriter = characterWriter;
        }

        public async Task<GetAllCharactersResponse> GetAllCharacters()
        {
            var characters = await _characterReader.GetAllCharacters();

            return new GetAllCharactersResponse
            {
                characters = characters,
                Status = HttpStatusCode.OK,
                Message = "Success, Returning Characters."
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
            throw new EntityNotFoundException(nameof(character), characterId);
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
            var character = await _characterReader.GetCharacter(characterId);
            if (character != null)
            {                           
                DamageServiceFactory factory = new DamageServiceFactory();
                IDamageService damageService = factory.GetDamageService(request.damageType);
                if (damageService == null)
                {
                    throw new EntityNotFoundException(nameof(request.damageType), request.damageType);

                }
                var _ = damageService.ApplyDamage(character, request.amount);
                character = _.Item1;
                var damageDealt = _.Item2;
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
    }
}
