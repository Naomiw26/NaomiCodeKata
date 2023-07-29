using RPGCombatKata.Domain.Data;

namespace RPGCombatKata.Api.Service.Services
{
    public abstract class IDamageService
    {
        public abstract (Character,int) ApplyDamage(Character character, int amount);
    }
}
