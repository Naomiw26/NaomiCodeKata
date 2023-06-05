using Microsoft.EntityFrameworkCore;
using RPGCombatKata.Data;

public class CharactersDb : DbContext
{
    public CharactersDb(DbContextOptions<CharactersDb> options)
        : base(options) { }

    public DbSet<CharacterRecord> Characters => Set<CharacterRecord>();
}