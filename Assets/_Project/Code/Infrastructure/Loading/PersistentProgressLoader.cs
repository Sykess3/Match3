using System;

namespace _Project.Code.Infrastructure.Loading
{
    public class PersistentProgressLoader : IPersistentProgressLoader
    {
        public void Load(Action onLoaded)
        {
            onLoaded?.Invoke();
        }
    }
}