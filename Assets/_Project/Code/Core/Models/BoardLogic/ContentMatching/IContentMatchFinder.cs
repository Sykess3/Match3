using System.Collections.Generic;
using System.Threading.Tasks;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.Directions;

namespace _Project.Code.Core.Models.BoardLogic.ContentMatching
{
    public interface IContentMatchFinder
    {
        List<Cell> FindMatch(Cell commandCell1);
        List<Cell> FindMatchesByWholeBoard();
    }
}