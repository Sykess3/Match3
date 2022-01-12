using System;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;

namespace _Project.Code.Core.Models.BoardLogic.Cells
{
    public interface ICellContentMover
    {
        void MoveCellContent(Cell from, Cell to, float speed, Action callback = null);
        void MoveCellContent(CellContent contentToMove, Cell to, float speed, Action callback = null);
    }
}