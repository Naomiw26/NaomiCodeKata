using Microsoft.EntityFrameworkCore;
using RPGCombatKata.Infrastructure;

namespace RPGCombatKata.Infrasturcture;
public class CharactersDb : DbContext
{
    public CharactersDb(DbContextOptions<CharactersDb> options)
        : base(options) { }

    public DbSet<CharacterRecord> Characters => Set<CharacterRecord>();
}