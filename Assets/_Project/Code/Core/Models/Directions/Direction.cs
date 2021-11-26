using System;
using UnityEngine;

namespace _Project.Code.Core.Models.Directions
{
    enum Direction
    {
        South,
        North,
        West,
        East
    }

    static class DirectionExtensions
    {
        public static Vector2 GetVector2(this Direction direction)
        {
            switch (direction)
            {
                case Direction.South:
                    return new Vector2(0, 1);
                case Direction.North:
                    return new Vector2(0, -1);
                case Direction.West:
                    return new Vector2(-1, 0);
                case Direction.East:
                    return new Vector2(1, 0);
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}