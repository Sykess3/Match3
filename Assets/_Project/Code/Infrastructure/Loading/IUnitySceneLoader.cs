using System;

namespace _Project.Code.Infrastructure.Loading
{
    public interface IUnitySceneLoader
    {
        void Load(string name, Action onLoaded = null);
    }
}