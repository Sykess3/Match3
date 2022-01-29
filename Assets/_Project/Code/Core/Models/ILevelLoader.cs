using System;

namespace _Project.Code.Core.Models
{
    public interface ILevelLoader
    {
        void Load(string name, Action onLoaded = null);
        void LoadLevelsField();
        void Reload(Action onLoaded = null);
    }
}