using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models.Cells;
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

        public List<Cell> Match(Cell cell)
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