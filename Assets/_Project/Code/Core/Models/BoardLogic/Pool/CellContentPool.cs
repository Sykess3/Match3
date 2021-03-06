using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.DataStructures;
using _Project.Code.Core.Models.Interfaces;

namespace _Project.Code.Core.Models.BoardLogic.Pool
{
    public class CellContentPool : ObjectPool<DefaultContentType, DefaultCellContent>, ICellContentPool
    {
        public CellContentPool(ICellContentFactory factory) : base(factory)
        {
        }
        
    }
}