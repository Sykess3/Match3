using UnityEngine;

namespace _Project.Code.Infrastructure.Loading
{
    internal class AssetProvider : IAssetProvider
    {
        public GameObject Load(string path) => Resources.Load<GameObject>(path);

        public T Load<T>(string path) where T : Component => Resources.Load<T>(path);
        public T Instantiate<T>(string path, Vector3 position = default) where T : Component
        {
            var prefab = Load<T>(path);
            var gameObject = Object.Instantiate(prefab.gameObject, position, Quaternion.identity);
            return gameObject.GetComponent<T>();
        }

        public T Instantiate<T>(GameObject prefab, Transform parent) => 
            Object.Instantiate(prefab, parent).GetComponent<T>();

        public T Instantiate<T>(T levelGoalPrefab, Transform parent) where T : Object => 
            Object.Instantiate<T>(levelGoalPrefab, parent);
    }
}