using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;

namespace _Project.Code.Core
{
    public interface ICellContentPool
    {
        CellContent Get(ContentType type);
    }
}