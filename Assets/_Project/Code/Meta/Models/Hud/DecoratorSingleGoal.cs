using _Project.Code.Core.Models.BoardLogic;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;

namespace _Project.Code.Meta.Models.Hud
{
    public class DecoratorSingleGoal : SingleGoal<DecoratorType>
    {
        public DecoratorSingleGoal(Board board, (DecoratorType, int) goal) : base(board, goal)
        {
        }

        public override void Subscribe() => Board.DecoratedMatched += OnMatch;

        public override void CleanUp() => Board.DecoratedMatched -= OnMatch;
        private void OnMatch(CellContentBase obj) => TryCollect(obj.DecoratorType);
    }
}