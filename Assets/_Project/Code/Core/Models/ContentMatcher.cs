using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models.Directions;
using UnityEngine;
using Zenject;

namespace _Project.Code.Core.Models
{
    public class ContentMatcher : IContentMatcher
    {
        private const int MinContentToMatch = 3;
        private readonly Board _board;

        public ContentMatcher(Board board)
        {
            _board = board;
        }

        public IReadOnlyCollection<Cell> Match(Cell cell)
        {
            var matchedCells = new List<Cell>();
            var matchedInEast = MatchInDirection(cell, Direction.East);
            var matchedInWest = MatchInDirection(cell, Direction.West);
            var matchedInNorth = MatchInDirection(cell, Direction.North);
            var matchedInSouth = MatchInDirection(cell, Direction.South);

            if (matchedInEast.Count >= MinContentToMatch)
                matchedCells.AddRange(matchedInEast);

            if (matchedInNorth.Count >= MinContentToMatch)
                matchedCells.AddRange(matchedInNorth);

            if (matchedInWest.Count >= MinContentToMatch)
                matchedCells.AddRange(matchedInWest);

            if (matchedInSouth.Count >= MinContentToMatch)
                matchedCells.AddRange(matchedInSouth);

            if (matchedCells.Count > 0)
                matchedCells.Add(cell);

            return matchedCells;
        }

        private List<Cell> MatchInDirection(Cell initialCell, Direction direction)
        {
            var matchedCells = new List<Cell>();
            var nextContentPosition = initialCell.Position + direction.GetVector2();

            while (NextCellIsSwitchable(nextContentPosition, out var nextCell))
            {
                if (NextCellCanBeMatchedWithInitialCell(initialCell, nextCell))
                    return matchedCells;

                matchedCells.Add(nextCell);
                nextContentPosition += direction.GetVector2();
            }

            return matchedCells;
        }

        private static bool NextCellCanBeMatchedWithInitialCell(Cell cell, Cell nextCell)
        {
            return cell.Filler.MatchableContent.All(x => x != nextCell.Filler.Type);
        }

        private bool NextCellIsSwitchable(Vector2 nextContentPosition, out Cell nextCell)
        {
            return _board.TryGetCell(nextContentPosition, out nextCell) && nextCell.Filler.Switchable;
        }
    }
}