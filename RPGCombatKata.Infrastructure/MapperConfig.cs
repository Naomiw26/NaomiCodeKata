using AutoMapper;
using RPGCombatKata.Domain.Data;
using RPGCombatKata.Infrastructure;

namespace RPGCombatKata.Domain
{
    public class MapperConfig
    {
        public static Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Character, CharacterRecord>();
                cfg.CreateMap<CharacterRecord, Character>();
            });
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
