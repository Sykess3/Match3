using System.Collections.Generic;

namespace _Project.Code.Infrastructure.Services
{
    public interface IConfigProvider
    {
        T Load<T>(string path) where T : class;
        IEnumerable<T> LoadCollection<T>(string path) where T : class;
    }
}