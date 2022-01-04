using System.Collections.Generic;
using _Project.Code.Core.Models.BoardLogic.Cells;

namespace _Project.Code.Core.Models.BoardLogic.ContentMatching
{
    public class MatchData
    {
        public HashSet<ContentToSpawn> ContentToSpawn { get; set; } = new HashSet<ContentToSpawn>();
        public HashSet<Cell> MatchedCells { get; set; } = new HashSet<Cell>();

        public HashSet<Cell> MatchedCellsWithoutDuplicatesInContentToSpawn
        {
            get
            {
                if (ContentToSpawn.Count == 0)
                    return MatchedCells;


                var result = new HashSet<Cell>();
                foreach (var matchedCell in MatchedCells)
                {
                    bool isInContentToSpawn = false;
                    
                    foreach (var toSpawn in ContentToSpawn)
                        if (matchedCell.Position == toSpawn.Position)
                            isInContentToSpawn = true;

                    if (!isInContentToSpawn) 
                        result.Add(matchedCell);
                }

                return result;
            }
        }
    }
}