using System;
using _Project.Code.Core.Models.BoardLogic.Cells;

namespace _Project.Code.Core.Models.BoardLogic
{
    public interface ICellContentFalling
    {
        void FillContentOnEmptyCell(Cell emptyCell, Action<Cell> onLandedCallback);
    }
}