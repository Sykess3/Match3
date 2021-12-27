using System.Linq;

namespace _Project.Code.Core.Models.BoardLogic.ContentMatching.FinderMiddlewareComponents
{
    public static class MatchDataExtensions
    {
        public static MatchData Concat(this MatchData first, MatchData second)
        {
            return new MatchData
            {
                MatchedCells = first.MatchedCells.Concat(second.MatchedCells).ToList(),
                ContentToSpawn = first.ContentToSpawn.Concat(second.ContentToSpawn).ToList()
            };
        }

        public static MatchData Distinct(this MatchData obj)
        {
            return new MatchData()
            {
                MatchedCells = obj.MatchedCells.Distinct().ToList(),
                ContentToSpawn = obj.ContentToSpawn
            };
        }
    }
}