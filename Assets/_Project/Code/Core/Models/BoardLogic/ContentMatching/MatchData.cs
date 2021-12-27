using System.Collections.Generic;
using _Project.Code.Core.Models.BoardLogic.Cells;

namespace _Project.Code.Core.Models.BoardLogic.ContentMatching
{
    public class MatchData
    {
        public List<ContentToSpawn> ContentToSpawn { get; set; } = new List<ContentToSpawn>();
        public List<Cell> MatchedCells { get; set; } = new List<Cell>();

        public IEnumerable<Cell> MatchedCellsWithoutDuplicatesInContentToSpawn
        {
            get
            {
                if (ContentToSpawn.Count == 0)
                {
                    foreach (var cell in MatchedCells)
                        yield return cell;
                }

                for (int i = 0; i < MatchedCells.Count; i++)
                {
                    for (int j = 0; j < ContentToSpawn.Count; j++)
                    {
                        if (MatchedCells[i].Position != ContentToSpawn[j].Position)
                        {
                            yield return MatchedCells[i];
                        }
                    }
                }
            }
        }
    }
}