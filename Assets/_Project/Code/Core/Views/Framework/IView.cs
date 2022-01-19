using System;
using UnityEngine;

namespace _Project.Code.Core.Views.Framework
{
    public interface IView
    {
        event Action Destroyed;
        event Action Created;

        GameObject gameObject { get; }
    }
}