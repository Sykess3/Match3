using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
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
            GetMatches(cell, out HashSet<Cell> matchedInVertical, out HashSet<Cell> matchedInHorizontal);

            if (matchedInVertical.Count >= Constant.MinContentToMatch - 1) 
                matchData.MatchedInVertical.UnionWith(matchedInVertical);

            if (matchedInHorizontal.Count >= Constant.MinContentToMatch - 1) 
                matchData.MatchedInHorizontal.UnionWith(matchedInHorizontal);

            if (matchData.GetAll.Count != 0) 
                matchData.MovedCell = cell;

            return matchData;
        }

        private void GetMatches(Cell cell, out HashSet<Cell> matchedInVertical, out HashSet<Cell> matchedInHorizontal)
        {
            var matchedInEast = MatchInDirection(cell, Direction.East);
            var matchedInWest = MatchInDirection(cell, Direction.West);
            var matchedInNorth = MatchInDirection(cell, Direction.North);
            var matchedInSouth = MatchInDirection(cell, Direction.South);


            matchedInVertical = new HashSet<Cell>();
            matchedInVertical.UnionWith(matchedInNorth);
            matchedInVertical.UnionWith(matchedInSouth);

            matchedInHorizontal = new HashSet<Cell>();
            matchedInHorizontal.UnionWith(matchedInEast);
            matchedInHorizontal.UnionWith(matchedInWest);
        }

        private HashSet<Cell> MatchInDirection(Cell initialCell, Direction direction)
        {
            var matchedCells = new HashSet<Cell>();
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
            
            public HashSet<Cell> MatchedInVertical { get; }
            public HashSet<Cell> MatchedInHorizontal { get; }
            public Cell MovedCell { get; set; }


            public MatchData()
            {
                MatchedInVertical = new HashSet<Cell>();
                MatchedInHorizontal = new HashSet<Cell>();
            }    
            
            public HashSet<Cell> GetAll
            {
                get
                {
                    var result = new HashSet<Cell>();
                    result.UnionWith(MatchedInHorizontal);
                    result.UnionWith(MatchedInVertical);
                    
                    if (MovedCell != null) 
                        result.Add(MovedCell);
                    
                    return result;
                }
            }
        }
    }
    
}