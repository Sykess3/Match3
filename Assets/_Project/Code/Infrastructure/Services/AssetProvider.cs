using UnityEngine;

namespace _Project.Code.Infrastructure.Services
{
    public class AssetProvider : IAssetProvider
    {
        public GameObject Load(string path) => Resources.Load<GameObject>(path);

        public T Load<T>(string path) where T : Component => Resources.Load<T>(path);
        public T Instantiate<T>(string path, Vector3 position = default) where T : Component
        {
            var component = Load<T>(path);
            var gameObject = Object.Instantiate(component.gameObject, position, Quaternion.identity);
            return gameObject.GetComponent<T>();
        }
    }
}