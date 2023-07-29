using AutoMapper;
using RPGCombatKata.Domain.Data;
using RPGCombatKata.Infrastructure;

namespace RPGCombatKata.Domain
{
    public class CharacterProfile : Profile
    {
        public CharacterProfile() 
        {
            CreateMap<Character, CharacterRecord>();
            CreateMap<CharacterRecord, Character>();
        }     
    }
}
