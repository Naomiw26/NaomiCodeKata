using RPGCombatKata.Api.Service.Exceptions;
using RPGCombatKata.Domain.Data;

namespace RPGCombatKata.Api.Service.Services
{
    public class HealingDamageService : IDamageService
    {
        public override (Character, int) ApplyDamage(Character character, int amount)
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
                return (character, damageDealt);
            } 
            throw new CharacterIsDeadWhenHealedException();        
        }    
    }

}
