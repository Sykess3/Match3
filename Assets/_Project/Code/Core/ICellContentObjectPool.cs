using _Project.Code.Core.Models.BoardLogic.Cells;

namespace _Project.Code.Core
{
    public interface ICellContentObjectPool
    {
        CellContent Get(ContentType type);
    }
}