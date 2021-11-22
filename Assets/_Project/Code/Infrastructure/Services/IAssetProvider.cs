using _Project.Code.Infrastructure.Installers.Scene;
using UnityEngine;

namespace _Project.Code.Infrastructure.Services
{
    public interface IAssetProvider
    {
        GameObject Load(string path);
        T Load<T>(string path) where T : Component;
        T Instantiate<T>(string path, Vector3 position = default) where T : Component;
    }
}