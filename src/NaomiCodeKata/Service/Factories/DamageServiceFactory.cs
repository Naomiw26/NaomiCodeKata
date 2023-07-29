using RPGCombatKata.Api.Data;
using RPGCombatKata.Api.Service.Services;

namespace RPGCombatKata.Api.Service.Factories
{
    public class DamageServiceFactory
    {
        public IDamageService GetDamageService(DamageType damageType)
        {

            switch (damageType)
            {
                case DamageType.Normal:
                    return new NormalDamageService();
                case DamageType.Healing:
                    return new HealingDamageService();
                default:
                    throw new ArgumentException("Invalid type", "type");
            }
        }
    }

}
