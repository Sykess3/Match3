using System.Collections.Generic;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;

namespace _Project.Code.Core.Models.BoardLogic.ContentMatching.FinderMiddlewareComponents
{
    public class DecoratorsFinder
    {
        private readonly CellCollection _cellCollection;

        public DecoratorsFinder(CellCollection cellCollection)
        {
            _cellCollection = cellCollection;
        }

        /// <summary>
        /// Removes decorated content from argument HashSet and return their.
        /// </summary>
        /// <param name="matchedCells"></param>
        public HashSet<Cell> Filter(HashSet<Cell> matchedCells)
        {
            HashSet<Cell> decoratedCells = new HashSet<Cell>();
            foreach (Cell matchedCell in matchedCells)
            {
                if (matchedCell.Content.IsDecorated) 
                    decoratedCells.Add(matchedCell);
            }
            
            matchedCells.ExceptWith(decoratedCells);

            return decoratedCells;
        }

        public HashSet<Cell> FindDecoratedNeighboursOf(HashSet<Cell> matchedCells)
        {
            HashSet<Cell> decoratedNeighbours = new HashSet<Cell>();
            foreach (var matchedCell in matchedCells)
            {
                HashSet<Cell> decoratedNeighboursOfCurrentCell = HitDecoratedNeighboursOf(matchedCell);
                decoratedNeighbours.UnionWith(decoratedNeighboursOfCurrentCell);
            }

            return decoratedNeighbours;
        }

        private HashSet<Cell> HitDecoratedNeighboursOf(Cell matchedCell)
        {
            List<Cell> neighboursOfMatchedCell = _cellCollection.GetNeighboursOf(matchedCell);
            HashSet<Cell> decoratedCells = new HashSet<Cell>();
            foreach (var cell in neighboursOfMatchedCell)
            {
                if (cell.Content.MatchType != DefaultContentType.Stone && cell.Content.IsDecorated)
                {
                    decoratedCells.Add(cell);
                }
            }

            return decoratedCells;
        }
    }
}