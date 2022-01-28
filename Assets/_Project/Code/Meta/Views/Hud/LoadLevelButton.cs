using _Project.Code.Core.Models;
using _Project.Code.Core.Models.Framework;

namespace _Project.Code.Meta.Views.Hud
{
    public abstract class LoadLevelButton : IModel
    {
        protected readonly ILevelLoader LevelLoader;

        protected LoadLevelButton(ILevelLoader levelLoader) => LevelLoader = levelLoader;
        public abstract void Load();
    }
}