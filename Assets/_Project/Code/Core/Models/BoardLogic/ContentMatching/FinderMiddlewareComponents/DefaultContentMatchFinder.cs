using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.Directions;
using UnityEngine;

namespace _Project.Code.Core.Models.BoardLogic.ContentMatching.FinderMiddlewareComponents
{
    public class DefaultContentMatchFinder 
    {
        private readonly CellCollection _cells;

        public DefaultContentMatchFinder(CellCollection cells)
        {
            _cells = cells;
        }
        
        public MatchData Find(Cell cell)
        {
            var matchData = new MatchData();
            GetMatches(cell, out List<Cell> matchedInVertical, out List<Cell> matchedInHorizontal);

            if (matchedInVertical.Count >= Constant.MinContentToMatch - 1) 
                matchData.MatchedInVertical.AddRange(matchedInVertical);

            if (matchedInHorizontal.Count >= Constant.MinContentToMatch - 1) 
                matchData.MatchedInHorizontal.AddRange(matchedInHorizontal);

            if (matchData.GetAll.Count != 0) 
                matchData.MovedCell = cell;

            return matchData;
        }

        private void GetMatches(Cell cell, out List<Cell> matchedInVertical, out List<Cell> matchedInHorizontal)
        {
            var matchedInEast = MatchInDirection(cell, Direction.East);
            var matchedInWest = MatchInDirection(cell, Direction.West);
            var matchedInNorth = MatchInDirection(cell, Direction.North);
            var matchedInSouth = MatchInDirection(cell, Direction.South);

            matchedInVertical = matchedInNorth
                .Concat(matchedInSouth)
                .ToList();
            matchedInHorizontal = matchedInEast
                .Concat(matchedInWest)
                .ToList();
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
        
        private static bool NextCellCanNotBeMatchedWithInitialCell(Cell cell, Cell nextCell) => 
            cell.Content.MatchableContent.All(x => x != nextCell.Content.Type);

        private bool NextCellNotStone(Vector2 nextContentPosition, out Cell nextCell) => 
            _cells.TryGetCell(nextContentPosition, out nextCell) && nextCell.Content.Type != ContentType.Stone;
        
        public class MatchData
        {
            
            public List<Cell> MatchedInVertical { get; }
            public List<Cell> MatchedInHorizontal { get; }
            public Cell MovedCell { get; set; }


            public MatchData()
            {
                MatchedInVertical = new List<Cell>();
                MatchedInHorizontal = new List<Cell>();
            }    
            
            public List<Cell> GetAll
            {
                get
                {
                    var cells = MatchedInHorizontal.Concat(MatchedInVertical).ToList();
                    if (MovedCell != null) 
                        cells.Add(MovedCell);
                    
                    return cells;
                }
            }
        }
    }
    
}