using UnityEngine;

namespace _Project.Code.Infrastructure.Services
{
    public class AssetProvider : IAssetProvider
    {
        public GameObject Instantiate(string path)
        {
            var gameObject = Resources.Load<GameObject>(path);
            return Object.Instantiate(gameObject);
        }

        public GameObject Instantiate(string path, Vector3 at)
        {
            var gameObject = Resources.Load<GameObject>(path);
            return Object.Instantiate(gameObject, at, Quaternion.identity);
        }

        public GameObject Instantiate<T>(string path) where T : Component
        {
            var component = Resources.Load<T>(path);
            return Object.Instantiate(component.gameObject);
        }

        public GameObject Instantiate<T>(string path, Vector3 at) where T : Component
        {
            var component = Resources.Load<T>(path);
            return Object.Instantiate(component.gameObject, at, Quaternion.identity);
        }
        
    }
}