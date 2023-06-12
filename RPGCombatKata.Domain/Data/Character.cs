namespace RPGCombatKata.Domain.Data
{
    public class Character
    {
        public Guid id { get; set; }
        public static int MaxHealth = 1000;
        public int health = MaxHealth;
        public int level = 1;
        public bool isAlive { get => health > 0 ? true : false; }

    }
}
