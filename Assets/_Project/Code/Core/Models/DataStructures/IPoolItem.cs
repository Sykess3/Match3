using System;

namespace _Project.Code.Core.Models.DataStructures
{
    public interface IPoolItem<out TArg>
    {
        event EventHandler Disabled;
        void Enable();
        TArg Type { get; }
    }
}