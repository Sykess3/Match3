using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using UnityEngine;

namespace _Project.Code.Core.Models.BoardLogic.ContentMatching
{
    public class ContentToSpawn
    {
        public DefaultContentType Type { get; }
        public Vector2 Position { get; }

        public ContentToSpawn(DefaultContentType type, Vector2 position)
        {
            Type = type;
            Position = position;
        }
    }
}