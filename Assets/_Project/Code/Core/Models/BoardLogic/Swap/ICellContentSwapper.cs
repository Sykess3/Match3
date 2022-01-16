using System;
using _Project.Code.Core.Models.BoardLogic.Cells;

namespace _Project.Code.Core.Models.BoardLogic.Swap
{
    public interface ICellContentSwapper
    {
        void SwapContent(Cell firstCell, Cell secondCell, Action callback = null);
    }
}