using System;

namespace _Project.Code.Infrastructure.Loading
{
    public interface IPersistentProgressLoader
    {
        void Load(Action onLoaded);
    }
}