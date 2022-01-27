using UnityEngine;

namespace _Project.Code.Infrastructure.Loading
{
    public interface IAssetProvider
    {
        GameObject Load(string path);
        T Load<T>(string path) where T : Component;
        T Instantiate<T>(string path, Vector3 position = default) where T : Component;
        T Instantiate<T>(GameObject prefab, Transform parent);
        T Instantiate<T>(T levelGoalPrefab, Transform transform) where T : Object;
    }
}