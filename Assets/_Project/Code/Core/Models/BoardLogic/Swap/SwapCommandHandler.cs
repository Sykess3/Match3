using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Input;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.ContentMatching;
using _Project.Code.Core.Models.BoardLogic.ContentMatching.FinderMiddlewareComponents;

namespace _Project.Code.Core.Models.BoardLogic.Swap
{
    public class SwapCommandHandler
    {
        private readonly IContentMatchFinder _matchFinder;
        private readonly ICellContentSwapper _cellContentSwapper;
        private readonly IPlayerInput _playerInput;
        private Action _onSwapSucceed;

        public event Action<MatchData> Matched;

        public SwapCommandHandler(ICellContentSwapper cellContentSwapper, IPlayerInput playerInput, IContentMatchFinder matchFinder)
        {
            _cellContentSwapper = cellContentSwapper;
            _playerInput = playerInput;
            _matchFinder = matchFinder;
        }

        public void Swap(SwapCommand command, Action onSwapSucceed)
        {
            _onSwapSucceed = onSwapSucceed;
            _playerInput.Disable();
            command.Swapper = _cellContentSwapper;
            command.Execute(RevertOrMatch);
        }

        private void RevertOrMatch(SwapCommand command)
        {
            var matchData = GetMatchedDataFromCommand(command);
            if (matchData.MatchedCells.Count == 0)
            {
                command.Revert(OnCommandReverted);
            }
            else
            {
                Matched?.Invoke(matchData);
                _onSwapSucceed();
            }
        }
        
        private void OnCommandReverted(SwapCommand obj) => _playerInput.Enable();
        
        
        private MatchData GetMatchedDataFromCommand(SwapCommand command)
        {
            var matchDataFirstCell = _matchFinder.FindMatch(command.FirstCell);
            var matchDataSecondCell = _matchFinder.FindMatch(command.SecondCell);

            return matchDataFirstCell
                .Concat(matchDataSecondCell);
        }
    }
}