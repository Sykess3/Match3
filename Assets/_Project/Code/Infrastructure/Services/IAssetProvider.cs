using UnityEngine;

namespace _Project.Code.Infrastructure.Services
{
    public interface IAssetProvider
    {
        GameObject Instantiate(string path);
        GameObject Instantiate(string path, Vector3 at);
        GameObject Instantiate<T>(string path) where T : Component;
        GameObject Instantiate<T>(string path, Vector3 at) where T : Component;
    }
}