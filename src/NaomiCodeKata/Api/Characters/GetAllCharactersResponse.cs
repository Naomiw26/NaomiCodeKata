using RPGCombatKata.Data;

namespace RPGCombatKata.Api.Characters
{
    public class GetAllCharactersResponse : BaseResponse
    {
        public List<Character> characters { get; set; }
    }
}
