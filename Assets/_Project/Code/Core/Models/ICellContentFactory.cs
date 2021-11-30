using _Project.Code.Core.Models.BoardLogic.Cells;
using UnityEngine;

namespace _Project.Code.Core.Models
{
    public interface ICellContentFactory
    {
        CellContent Create(ContentType type, Vector2 position);
    }
}