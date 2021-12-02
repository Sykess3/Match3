using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.Directions;
using UnityEngine;

namespace _Project.Code.Core.Models.BoardLogic.ContentMatching
{
    public class ContentMatchFinder : IContentMatchFinder
    {
        private readonly CellCollection _cells;
        private const int MinContentToMatch = 3;

        public ContentMatchFinder(CellCollection cells)
        {
            _cells = cells;
        }
        
        public List<Cell> FindMatch(Cell cell)
        {
            var matchedCells = new List<Cell>();
            var matchedInEast = MatchInDirection(cell, Direction.East);
            var matchedInWest = MatchInDirection(cell, Direction.West);
            var matchedInNorth = MatchInDirection(cell, Direction.North);
            var matchedInSouth = MatchInDirection(cell, Direction.South);

            var matchedInVertical = matchedInNorth
                .Concat(matchedInSouth);
            var matchedInHorizontal = matchedInEast
                .Concat(matchedInWest);

            if (matchedInVertical.Count() >= MinContentToMatch - 1)
                matchedCells.AddRange(matchedInVertical);

            if (matchedInHorizontal.Count() >= MinContentToMatch - 1)
                matchedCells.AddRange(matchedInHorizontal);

            if (matchedCells.Count > 0)
                matchedCells.Add(cell);

            return OrderMatchedCells(matchedCells);
        }

        public List<Cell> FindMatchesByWholeBoard()
        {
            List<Cell> allMatchedCells = new List<Cell>(8);
            foreach (var cell in _cells.GetAll())
            {
                if (IsCellNotMatchedYet(allMatchedCells, cell))
                {
                    var matchedCells = FindMatch(cell);
                    if (matchedCells.Count != 0) 
                        allMatchedCells.AddRange(matchedCells);
                }
            }

            return OrderMatchedCells(allMatchedCells.Distinct());
        }

        private List<Cell> MatchInDirection(Cell initialCell, Direction direction)
        {
            var matchedCells = new List<Cell>();
            var nextContentPosition = initialCell.Position + direction.GetVector2();

            while (NextCellNotStone(nextContentPosition, out var nextCell))
            {
                if (NextCellCanNotBeMatchedWithInitialCell(initialCell, nextCell))
                    return matchedCells;

                matchedCells.Add(nextCell);
                nextContentPosition += direction.GetVector2();
            }

            return matchedCells;
        }

        private static List<Cell> OrderMatchedCells(IEnumerable<Cell> matchedCells)
        {
            return matchedCells.OrderBy(x => x.Position.y)
                .ToList();
        }

        private static bool NextCellCanNotBeMatchedWithInitialCell(Cell cell, Cell nextCell) => 
            cell.Content.MatchableContent.All(x => x != nextCell.Content.Type);

        private bool NextCellNotStone(Vector2 nextContentPosition, out Cell nextCell) => 
            _cells.TryGetCell(nextContentPosition, out nextCell) && nextCell.Content.Type != ContentType.Stone;

        private static bool IsCellNotMatchedYet(List<Cell> allMatchedCells, Cell cell)
        {
            foreach (Cell alreadyMatchedCell in allMatchedCells)
                if (cell == alreadyMatchedCell)
                    return false;
            return true;
        }
    }
}