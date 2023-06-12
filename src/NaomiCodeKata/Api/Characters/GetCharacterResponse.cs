using RPGCombatKata.Data;

namespace RPGCombatKata.Api.Characters
{
    public class GetCharacterResponse : BaseResponse
    {
        public Character character { get; set; }
    }
}
