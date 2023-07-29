namespace RPGCombatKata.Api.Service.Exceptions
{
    public class CharacterIsDeadWhenHealedException : Exception
    {
        public CharacterIsDeadWhenHealedException()
            : base($"Character is Dead and Can't be Healed.")
        {
        }
    }
}
