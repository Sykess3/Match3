using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using UnityEngine;

namespace _Project.Code.Core.Models.Random
{
    public interface IRandomCellContentGenerator
    {
        CellContent Generate(Vector2 position);
    }
}