﻿namespace RPGCombatKata.Infrastructure
{
    public class CharacterRecord
    {
        public Guid id { get; set; }
        public int health { get; set; }
        public int level = 1;
    }
}