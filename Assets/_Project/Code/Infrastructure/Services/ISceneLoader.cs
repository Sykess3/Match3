using System;

namespace _Project.Code.Infrastructure.Services
{
    public interface ISceneLoader
    {
        void Load(string name, Action onLoaded = null);
    }
}