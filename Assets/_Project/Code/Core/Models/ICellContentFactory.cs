using _Project.Code.Core.Models.Cells;

namespace _Project.Code.Core.Models
{
    public interface ICellContentFactory
    {
        Cell.Content Create(Cell.ContentType type);
    }
}