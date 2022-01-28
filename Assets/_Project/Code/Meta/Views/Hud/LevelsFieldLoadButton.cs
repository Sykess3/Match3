using _Project.Code.Core.Models;

namespace _Project.Code.Meta.Views.Hud
{
    public class LevelsFieldLoadButton : LoadLevelButton
    {
        public LevelsFieldLoadButton(ILevelLoader levelLoader) : base(levelLoader)
        {
        }

        public override void Load() => LevelLoader.LoadLevelsField();
    }
}