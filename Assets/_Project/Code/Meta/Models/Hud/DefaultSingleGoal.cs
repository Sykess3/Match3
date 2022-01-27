using _Project.Code.Core.Models.BoardLogic;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;

namespace _Project.Code.Meta.Models.Hud
{
    public class DefaultSingleGoal : SingleGoal<DefaultContentType>
    {
        public DefaultSingleGoal(Board board, (DefaultContentType, int) goal) : base(board, goal)
        {
        }

        public override void Subscribe() => Board.DefaultMatched += OnMatch;

        public override void CleanUp() => Board.DefaultMatched -= OnMatch;

        private void OnMatch(CellContentBase obj) => TryCollect(obj.MatchType);
    }
}