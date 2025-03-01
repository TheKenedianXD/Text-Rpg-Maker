using TextRpg.Core.Models.Data.Character;
using TextRpg.Core.Services.Game;

namespace TextRpg.Core.Models.Player
{
    public class PlayerModel
    {
        public CharacterModel Character { get; set; }

        public PlayerModel(CharacterModel character)
        {
            Character = character;
            StatService.Initialize(character);
        }
    }
}
