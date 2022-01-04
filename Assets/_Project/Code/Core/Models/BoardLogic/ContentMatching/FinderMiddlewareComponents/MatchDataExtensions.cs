using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models.BoardLogic.Cells;

namespace _Project.Code.Core.Models.BoardLogic.ContentMatching.FinderMiddlewareComponents
{
    public static class MatchDataExtensions
    {
        public static MatchData Concat(this MatchData first, MatchData second)
        {
            HashSet<Cell> matchedCells = new HashSet<Cell>(first.MatchedCells);
            matchedCells.UnionWith(second.MatchedCells);

            HashSet<ContentToSpawn> contentToSpawn = new HashSet<ContentToSpawn>(first.ContentToSpawn);
            contentToSpawn.UnionWith(second.ContentToSpawn);
            return new MatchData
            {
                MatchedCells = matchedCells,
                ContentToSpawn = contentToSpawn
            };
        }
    }
}