using _Project.Code.Core.Models.Cells;
using UnityEngine;

namespace _Project.Code.Core.Models
{
    public interface IRandomCellContentGenerator
    {
        CellContent Generate(Vector2 position);
    }
}