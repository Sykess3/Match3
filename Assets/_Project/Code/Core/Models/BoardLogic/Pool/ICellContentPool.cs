using _Project.Code.Core.Models.BoardLogic.Cells.Content;

namespace _Project.Code.Core.Models.BoardLogic.Pool
{
    public interface ICellContentPool
    {
        CellContent Get(ContentType type);
    }
}