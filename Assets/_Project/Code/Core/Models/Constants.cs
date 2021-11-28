using UnityEngine;

namespace _Project.Code.Core.Models
{
    public static class Constants
    {
        public static class Board
        {
            public static Vector2 OffsetFromCenter { get; private set; }
            public static void InitializeOffset(Vector2Int boardSize)
            {
                OffsetFromCenter = new Vector2((boardSize.x - 1)
                                               * 0.5f, (boardSize.y - 1) * 0.5f);
            }
        }
    }
}