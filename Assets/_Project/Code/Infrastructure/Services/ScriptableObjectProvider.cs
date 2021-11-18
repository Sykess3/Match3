using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Code.Infrastructure.Services
{
    public class ScriptableObjectProvider : IConfigProvider
    {
        public T Load<T>(string path) where T : class
        {
            var config = Resources.Load<ScriptableObject>(path);
            return config as T;
        }

        public IEnumerable<T> LoadCollection<T>(string path) where T : class
        {
            var configs = Resources.LoadAll<ScriptableObject>(path);
            return configs.Cast<T>();
        }
    }
}