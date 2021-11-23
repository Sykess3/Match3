using _Project.Code.Core.Models.Cells;

namespace _Project.Code.Core.Models
{
    public interface IRandomCellContentGenerator
    {
        Cell.Content Generate();
    }
}