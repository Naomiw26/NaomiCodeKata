using RPGCombatKata.Domain.Data;

namespace RPGCombatKata.Api.Service.Services
{
    public class NormalDamageService : IDamageService
    {
        public override (Character,int) ApplyDamage(Character character, int amount)
        {
            character.health -= amount;
            var damageDealt = amount;
            if (character.health < 0)
            {
                damageDealt += character.health;
                character.health = 0;
            }
            return (character, damageDealt);
        } 
    }

}
