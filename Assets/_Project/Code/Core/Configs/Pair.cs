using System;
using System.Collections.Generic;

namespace _Project.Code.Core.Configs
{
    [Serializable]
    public class Pair<T1,T2>
    {
        public T1 Member1;
        public T2 Member2;
    }
}