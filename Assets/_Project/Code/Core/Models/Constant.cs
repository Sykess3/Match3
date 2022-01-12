using UnityEngine;

namespace _Project.Code.Core.Models
{
    public static class Constant
    {
        public const int MinContentToMatch = 3;
        public const int MinContentToUp = 6;
        public const float Tolerance = 0.01f;

        public static class Board
        {
            public static Vector2Int Size => new Vector2Int(9, 9);
            public static Vector2 OffsetFromCenter => new Vector2((Size.x - 1)
                                                                  * 0.5f, (Size.y - 1) * 0.5f);

            public static int CellCount => Size.x * Size.y;
        }
    }
}