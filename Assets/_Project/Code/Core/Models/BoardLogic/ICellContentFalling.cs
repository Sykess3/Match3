using System;
using _Project.Code.Core.Models.BoardLogic.Cells;

namespace _Project.Code.Core.Models.BoardLogic
{
    public interface ICellContentFalling
    {
        bool TryFillContentOnEmptyCell(Cell emptyCell, Action<Cell> onLandedCallback);
    }
}