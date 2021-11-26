using System;
using _Project.Code.Core.Models.Cells;

namespace _Project.Code.Core.Models
{
    public interface ICellContentMover
    {
        void MoveCellContent(Cell from, Cell to, float speed, Action callback = null);
        void MoveCellContent(CellContent contentToMove, Cell to, float speed, Action callback = null);
    }
}