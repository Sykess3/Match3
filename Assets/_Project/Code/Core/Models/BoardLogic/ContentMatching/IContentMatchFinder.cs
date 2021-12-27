using System.Collections.Generic;
using _Project.Code.Core.Models.BoardLogic.Cells;

namespace _Project.Code.Core.Models.BoardLogic.ContentMatching
{
    public interface IContentMatchFinder
    {
        MatchData FindMatch(Cell commandCell1);
        MatchData FindMatchesByWholeBoard();
    }
}