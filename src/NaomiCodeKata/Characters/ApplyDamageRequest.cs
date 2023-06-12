using RPGCombatKata.Api.Data;

namespace RPGCombatKata.Api.Characters
{
    public class ApplyDamageRequest
    {
        public int amount { get; set; }
        public DamageType damageType { get; set; }
    }
}
