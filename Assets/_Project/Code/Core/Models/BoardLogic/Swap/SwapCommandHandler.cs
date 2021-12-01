using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Input;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.ContentMatching;

namespace _Project.Code.Core.Models.BoardLogic.Swap
{
    public class SwapCommandHandler
    {
        private readonly ContentMatcher _matcher;
        private readonly ICellContentSwapper _cellContentSwapper;
        private readonly IPlayerInput _playerInput;

        public event Action<IEnumerable<Cell>> Matched;

        public SwapCommandHandler(ContentMatcher matcher, ICellContentSwapper cellContentSwapper, IPlayerInput playerInput)
        {
            _matcher = matcher;
            _cellContentSwapper = cellContentSwapper;
            _playerInput = playerInput;
        }

        public void Swap(SwapCommand command)
        {
            _playerInput.Disable();
            command.Swapper = _cellContentSwapper;
            command.Execute(OnCommandExecuted);
        }

        private void OnCommandExecuted(SwapCommand command)
        {
            if (!_matcher.TryMatch(command, OnMatched))
                command.Revert(OnCommandReverted);
        }

        private void OnMatched(IEnumerable<Cell> obj) => Matched?.Invoke(obj);

        private void OnCommandReverted(SwapCommand obj) => _playerInput.Enable();
    }
}