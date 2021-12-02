using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public bool TryMatch(SwapCommand command, Action<IEnumerable<Cell>> onMatched)
        {
            List<Cell> matchedCells =  GetMatchedCellsFromCommand(command)
                .OrderBy(x => x.Position.y)
                .ToList();

            if (matchedCells.Count > 0)
            {
                DestroyCells(matchedCells, onMatched);
                return true;
            }
            return false;
        }

        public async void ResolveMatchesByWholeBoard(Action<IEnumerable<Cell>> onMatched)
        {
            var matchesByWholeBoard = await Task.Run(_matchFinder.FindMatchesByWholeBoard);
            DestroyCells(matchesByWholeBoard, onMatched);
        }

        private List<Cell> GetMatchedCellsFromCommand(SwapCommand command)
        {
            List<Cell> matchedCellsAroundFirstCell = _matchFinder.FindMatch(command.FirstCell);
            List<Cell> matchedCellsAroundSecondCell = _matchFinder.FindMatch(command.SecondCell);
            
            var matchedCells = matchedCellsAroundFirstCell.Union(matchedCellsAroundSecondCell).ToList();
            return matchedCells;
        }

        private void DestroyCells(IEnumerable<Cell> matchedCells, Action<IEnumerable<Cell>> onMatched)
        {
            foreach (Cell matchedCell in matchedCells) 
                matchedCell.Content.IsDestroying = true;
            
            onMatched?.Invoke(matchedCells);

            foreach (var cell in matchedCells) 
                cell.Content.Destroy();
        }
    }
}