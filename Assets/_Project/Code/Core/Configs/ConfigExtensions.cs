using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;

namespace _Project.Code.Core.Configs
{
    public static class ConfigExtensions
    {
        public static Dictionary<T1, T2> GetDictionary<T1, T2>(this IEnumerable<Pair<T1, T2>> pair) => 
            pair.ToDictionary(x => x.Member1, x => x.Member2);
    }
}