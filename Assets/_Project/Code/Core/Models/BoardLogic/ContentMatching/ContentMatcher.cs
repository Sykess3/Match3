using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Swap;

namespace _Project.Code.Core.Models.BoardLogic.ContentMatching
{
    public class ContentMatcher
    {
        private readonly IContentMatchFinder _matchFinder;

        public ContentMatcher(IContentMatchFinder matchFinder)
        {
            _matchFinder = matchFinder;
        }
        public bool TryMatch(SwapCommand command)
        {
            List<Cell> matchedCells =  GetMatchedCellsFromCommand(command);

            if (matchedCells.Count > 0)
            {
                DestroyCells(matchedCells);
                return true;
            }
            return false;
        }

        private List<Cell> GetMatchedCellsFromCommand(SwapCommand command)
        {
            List<Cell> matchedCellsAroundFirstCell = _matchFinder.FindMatch(command.FirstCell);
            List<Cell> matchedCellsAroundSecondCell = _matchFinder.FindMatch(command.SecondCell);
            
            var matchedCells = matchedCellsAroundFirstCell.Union(matchedCellsAroundSecondCell).ToList();
            return matchedCells;
        }

        private void DestroyCells(IEnumerable<Cell> matchedCells)
        {
            foreach (var cell in matchedCells) 
                cell.Content.Match();
        }
    }
}