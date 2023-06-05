namespace RPGCombatKata.Data
{
    public class Character
    {
        public Guid id { get; set; }
        public int health { get; set; }
        public static int MaxHealth = 100;
        public int level { get; set; }
        public bool isAlive { get; set; }
    }
}
