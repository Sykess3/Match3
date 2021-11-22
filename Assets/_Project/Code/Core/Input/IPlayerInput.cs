using System;
using UnityEngine;

namespace _Project.Code.Core.Input
{
    public interface IPlayerInput
    {
        event Action<Vector2> ClickedOnPosition;
    }
}