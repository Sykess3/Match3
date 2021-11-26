using System;
using _Project.Code.Core.Models.Cells;

namespace _Project.Code.Core.Models
{
    public interface ICellContentSwapper
    {
        void SwapContent(Cell firstCell, Cell secondCell, float speed, Action callback = null);
    }
}